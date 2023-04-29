using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace GPTwitchBot.Twitch
{
    public class TwitchBot
    {
        string _token = string.Empty;
        string _botChannel = string.Empty;
        string _streamChannel = string.Empty;
        TwitchClient? _client;

        public string BotChannel { get => _botChannel; set => _botChannel = value; }

        private string _receivedMessage = string.Empty;
        public string ReceivedMessage { get => _receivedMessage; }

        private string _messageSender = string.Empty;
        public string MessageSender { get => _messageSender; }

        public event EventHandler? OnChatMentionMessageReceived;

        public void Start(string? token, string? bot, string? streamer)
        {
            if (token is not null && bot is not null && streamer is not null)
            {
                _token = token;
                _botChannel = bot.ToLower();
                _streamChannel = streamer.ToLower();
                _client = new TwitchClient();
                _client.Initialize(new ConnectionCredentials(_botChannel, _token), _streamChannel);
                _client.OnMessageReceived += Client_OnMentionMessageReceived;
                _client.Connect();    
            }
        }

        private void Client_OnMentionMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.ToLower().Contains(BotChannel))
            {
                _receivedMessage = e.ChatMessage.Message;
                _messageSender = e.ChatMessage.DisplayName;
                OnChatMentionMessageReceived?.Invoke(this, new EventArgs());
            }
        }

        public void Send(string message)
        {
            _client?.SendMessage(_client.GetJoinedChannel(_streamChannel), message);
        }

        public void Stop()
        {
            if (_client != null)
            {
                _client.OnMessageReceived -= Client_OnMentionMessageReceived;
                _client.Disconnect();
                _client = null;
            }
        }
    }
}

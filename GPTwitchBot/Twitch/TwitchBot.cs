using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace GPTwitchBot.Twitch
{
    public class TwitchBot
    {
        string _token = "";
        string _botChannel = "";
        string _streamChannel = "";
        TwitchClient _client;
        ConnectionCredentials _credentials;

        public string BotChannel { get => _botChannel; set => _botChannel = value; }

        private string _receivedMessage = null;
        public string ReceivedMessage { get => _receivedMessage; }

        private string messageSender = null;
        public string MessageSender { get => messageSender; }

        public event EventHandler OnChatMentionMessageReceived;

        public TwitchBot(string token, string bot, string streamer)
        {
            _token = token;
            _botChannel = bot.ToLower();
            _streamChannel = streamer.ToLower();
            _credentials = new ConnectionCredentials(BotChannel, _token);
        }

        public void Start()
        {
            _client = new TwitchClient();
            _client.Initialize(_credentials, _streamChannel);
            _client.OnMessageReceived += Client_OnMentionMessageReceived;
            _client.Connect();
        }

        private void Client_OnMentionMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.ToLower().Contains(BotChannel))
            {
                _receivedMessage = e.ChatMessage.Message;
                messageSender = e.ChatMessage.DisplayName;
                OnChatMentionMessageReceived.Invoke(this, new EventArgs());
            }
        }

        public void Send(string message)
        {
            _client.SendMessage(_client.GetJoinedChannel(_streamChannel), message);
        }

        public void Stop()
        {
            if (_client != null)
            {
                _client.Disconnect();
                _client = null;
            }
        }
    }
}

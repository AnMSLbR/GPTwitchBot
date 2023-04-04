using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace GPTwitchBot.Twitch
{
    internal class TwitchBot
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
        public event EventHandler OnChatQuestionMessageReceived;

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
            _client.OnMessageReceived += Client_OnQuestionMessageReceived;
            _client.Connect();
        }

        private void Client_OnQuestionMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.ToLower().Contains('?') && (e.ChatMessage.Message.ToLower().Contains(BotChannel) == false))
            {
                _receivedMessage = e.ChatMessage.Message;
                messageSender = e.ChatMessage.DisplayName;
                OnChatQuestionMessageReceived.Invoke(this, new EventArgs());
            }
        }

        private void Client_OnMentionMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.ToLower().Contains(BotChannel))
            {
                _receivedMessage = e.ChatMessage.Message.Substring(BotChannel.Length + 1);
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

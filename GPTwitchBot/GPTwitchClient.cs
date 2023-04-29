using GPTwitchBot.GPT;
using GPTwitchBot.Twitch;
using Microsoft.Extensions.Configuration;

namespace GPTwitchBot
{
    public class GPTwitchClient
    {
        private readonly IConfiguration _config;
        private readonly GPTClient _gptClient;
        private readonly TwitchBot _twitchBot;
        private Users _users;
        private string _answer = string.Empty;
        private Message? _message;
        private string _preset = string.Empty;

        public GPTwitchClient(IConfiguration config, GPTClient gptClient, TwitchBot twitchBot, Users users)
        {
            _config = config;
            _gptClient = gptClient;
            _twitchBot = twitchBot;
            _users = users;
        }

        public void Run()
        {
            _gptClient.Initialize(_config["chatGPT:apiKey"], _config["chatGPT:endPoint"]);
            _preset = _config["chatGPT:preset"] ?? string.Empty;
            _twitchBot.OnChatMentionMessageReceived += Bot_OnChatMentionMessageReceived;
            _twitchBot.Start(_config["twitch:token"], _config["twitch:botChannel"], _config["twitch:streamChannel"]);
            while (true)
            {
                if (true)
                {
                    var content = Console.ReadLine();
                    if (content?.ToLower() == "stop") break;
                }
            }
        }

        private async void Bot_OnChatMentionMessageReceived(object? sender, EventArgs e)
        {
            if (_twitchBot.MessageSender.ToLower() != _twitchBot.BotChannel)
            {
                string message = _preset + _twitchBot.ReceivedMessage.ToLower().Replace($"@{_twitchBot.BotChannel}", "").Replace($"{_twitchBot.BotChannel}", "").Trim();
                int index = _users.AddMessageToHistory(_twitchBot.MessageSender, message);
                await ReplyToMessageAsync(_users[index]);
            }
        }

        private async Task ReplyToMessageAsync(User user)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} {user.Name}: {user.ChatHistory.Last().Content.Replace(_preset, "")}");

            _answer = await _gptClient.ChatAsync(user.ChatHistory);
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} ChatGPT: {_answer}");
            while (_answer.Length > 250)
            {
                _message = new Message() { Role = "user", Content = "make it short" };
                user.ChatHistory.Add(_message);
                _answer = await _gptClient.ChatAsync(user.ChatHistory);
                Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} ChatGPT: {_answer}");
            }
            user.LastMessageDate = DateTime.Now;
            _twitchBot.Send("@" + user.Name + " " + _answer);
        }
    }
}

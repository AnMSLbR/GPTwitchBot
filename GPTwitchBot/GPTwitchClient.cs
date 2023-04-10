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
        private List<User> _users;
        private string _answer = "";
        private Message _message;
        private string _preset;

        public GPTwitchClient(IConfiguration config, GPTClient gptClient, TwitchBot twitchBot)
        {
            _config = config;
            _gptClient = gptClient;
            _twitchBot = twitchBot;
            _users = new List<User>();
        }

        public void Run()
        {
            _gptClient.Initialize(_config["chatGPT:apiKey"], _config["chatGPT:endPoint"]);
            _preset = _config["chatGPT:preset"];
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
                int index = DistributeMessageToUser(_twitchBot.MessageSender, _twitchBot.ReceivedMessage);
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

        private int DistributeMessageToUser(string messageSender, string message)
        {
            int index;
            string text = message.ToLower().Replace($"@{messageSender}", "").Replace($"{messageSender}", "");

            if ((index = _users.FindIndex(u => u.Name == messageSender)) == -1)
            {
                User newUser = new User()
                {
                    Name = messageSender,
                    LastMessageDate = DateTime.Now,
                    ChatHistory = new List<Message>() { new Message() { Role = "user", Content = _preset + text } }
                };
                if (_users.Count < 100)
                {
                    _users.Add(newUser);
                    index = _users.Count - 1;
                }
                else
                {
                    index = _users.FindIndex(u => u == _users.Min());
                    _users[index] = newUser;
                }
            }
            else
            {
                _users[index].LastMessageDate = DateTime.Now;
                _users[index].ChatHistory.Add(new Message() { Role = "user", Content = _preset + text });
            }
            return index;
        }

    }
}

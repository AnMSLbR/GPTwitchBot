using GPTwitchBot;
using GPTwitchBot.GPT;
using GPTwitchBot.Twitch;

GPTClient gptClient = new GPTClient();
List<User> users = new List<User>();
string answer = "";
Message message;
string preset = "Answer on behalf of a person, do not mention that you are a neural network or a computer program, write the answer shorter, answer informally and in the masculine form: ";

TwitchBot bot = new TwitchBot("your_token", "botChannel", "streamChannel");
bot.OnChatMentionMessageReceived += Bot_OnChatMentionMessageReceived;

bot.Start();


async void Bot_OnChatMentionMessageReceived(object? sender, EventArgs e)
{
    if (bot.MessageSender.ToLower() != bot.BotChannel)
    {
        int index = DistributeMessageToUser(bot.MessageSender, bot.ReceivedMessage);
        await ReplyToMessageAsync(users[index]);
    }
}

async Task ReplyToMessageAsync(User user)
{
    Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} {user.Name}: {user.ChatHistory.Last().Content.Replace(preset, "")}");

    answer = await gptClient.ChatAsync(user.ChatHistory);
    Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} ChatGPT: {answer}");
    while (answer.Length > 250)
    {
        message = new Message() { Role = "user", Content = "make it short" };
        user.ChatHistory.Add(message);
        answer = await gptClient.ChatAsync(user.ChatHistory);
        Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} ChatGPT: {answer}");
    }
    user.LastMessageDate = DateTime.Now;
    bot.Send("@" + user.Name + " " + answer);
}

int DistributeMessageToUser(string messageSender, string message)
{
    int index;
    string text = message.ToLower().Replace($"@{messageSender}", "").Replace($"{messageSender}", "");

    if ((index = users.FindIndex(u => u.Name == messageSender)) == -1)
    {
        User newUser = new User()
        {
            Name = messageSender,
            LastMessageDate = DateTime.Now,
            ChatHistory = new List<Message>() { new Message() { Role = "user", Content = preset + text } }
        };
        if (users.Count < 100)
        {
            users.Add(newUser);
            index = users.Count - 1;
        }
        else
        {
            index = users.FindIndex(u => u == users.Min());
            users[index] = newUser;
        }
    }
    else
    {
        users[index].LastMessageDate = DateTime.Now;
        users[index].ChatHistory.Add(new Message() { Role = "user", Content = preset + text });
    }
    return index;
}

while (true)
{
    var content = Console.ReadLine();
    if (content?.ToLower() == "stop") break;
}

bot.Stop();
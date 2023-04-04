using GPTwitchBot.GPT;
using GPTwitchBot.Twitch;

GPTClient gptClient = new GPTClient();
string answer = "";
Message message;
string preset = "Answer on behalf of a person, do not mention that you are a neural network or a computer program, write the answer shorter, answer informally and in the masculine form: ";

TwitchBot bot = new TwitchBot("your_token", "botChannel", "streamChannel");
bot.OnChatMentionMessageReceived += Bot_OnChatMentionMessageReceived;
bot.OnChatQuestionMessageReceived += Bot_OnChatQuestionMessageReceived;

bot.Start();


async void Bot_OnChatMentionMessageReceived(object? sender, EventArgs e)
{
    if (bot.MessageSender.ToLower() != bot.BotChannel)
    {
        await ReplyToMessageAsync(bot.MessageSender, bot.ReceivedMessage);
    }
}

async void Bot_OnChatQuestionMessageReceived(object? sender, EventArgs e)
{
    if (bot.MessageSender.ToLower() != bot.BotChannel && bot.ReceivedMessage.Contains('@') == false)
    {
        await ReplyToMessageAsync(bot.MessageSender, bot.ReceivedMessage);
    }
}

async Task ReplyToMessageAsync(string sender, string text)
{
    Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} {sender}: {text}");
    var content = preset + text;
    message = new Message() { Role = "user", Content = content };
    answer = await gptClient.ChatAsync(message);
    Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} ChatGPT: {answer}");
    while (answer.Length > 250)
    {
        message = new Message() { Role = "user", Content = "make it short" };
        answer = await gptClient.ChatAsync(message);
        Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} ChatGPT: {answer}");
    }
    bot.Send("@" + sender + " " + answer);
}

while (true)
{
    var content = Console.ReadLine();
    if (content?.ToLower() == "stop") break;
}

bot.Stop();
using GPTwitchBot.GPT;
using System.Net.Http.Json;
using System.Text.Json.Serialization;


GPTClient gPTClient = new GPTClient();

while (true)
{
    Console.Write("User: ");
    var content = Console.ReadLine();

    if (content is not { Length: > 0 }) break;
    var message = new Message() { Role = "user", Content = content };

    var answer = await gPTClient.ChatAsync(message);
    Console.WriteLine($"ChatGPT: {answer}");
}
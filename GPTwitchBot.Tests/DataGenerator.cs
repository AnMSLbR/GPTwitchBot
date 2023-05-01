using Bogus;
using GPTwitchBot.GPT;

namespace GPTwitchBot.Tests;

public class DataGenerator
{
    Faker<User> userModelFake;
    public DataGenerator()
    {
        Randomizer.Seed = new Random(111);
        userModelFake = new Faker<User>()
        .RuleFor(u => u.Name, f => f.Internet.UserName())
        .RuleFor(u => u.LastMessageDate, f => f.Date.Between(new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2010, 1, 1, 0, 0, 0)))
        .RuleFor(u => u.ChatHistory, f => new List<Message>() {new Message(){ Content = f.Lorem.Sentence()}});
    }

    public IEnumerable<User> GenerateUsers()
    {
        return userModelFake.GenerateForever();
    }
}

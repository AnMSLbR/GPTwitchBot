using GPTwitchBot;

namespace GPTwitchBot.Tests;

[TestClass]
public class UsersTest
{
    private Users? _users10;
    private Users? _users100;
    private DataGenerator? _dataGenerator;

    [TestInitialize]
    public void TestInitialize()
    {
        _dataGenerator = new DataGenerator();
        _users10 = new Users(_dataGenerator.GenerateUsers().Take(10));
        _users100 = new Users(_dataGenerator.GenerateUsers().Take(100));
    }

    [TestMethod]
    public void AddMessageToHistoryFromNewUser()
    {
        int expectedIndex = 10;
        string newUser = "NewUser";
        string newMessage = "New message";

        int actualIndex = _users10!.AddMessageToHistory(newUser, newMessage);

        Assert.AreEqual(expectedIndex, actualIndex);
        Assert.AreEqual(newUser, _users10![actualIndex].Name);
        Assert.AreEqual(newMessage, _users10![actualIndex].ChatHistory.Last().Content);
    }

    [TestMethod]
    public void AddMessageToHistoryFromExistingUser()
    {
        int expectedIndex = 6;
        string userName = _users10![expectedIndex].Name;
        string newMessage = "New message";

        int actualIndex = _users10.AddMessageToHistory(userName, newMessage);

        Assert.AreEqual(expectedIndex, actualIndex);
        Assert.AreEqual(userName, _users10![actualIndex].Name);
        Assert.AreEqual(newMessage, _users10![actualIndex].ChatHistory.Last().Content);
    }

    [TestMethod]
    public void AddMessageToHistoryWithFullListFromNewUser()
    {
        int expectedIndex = 7;
        string newUser = "NewUser";
        string newMessage = "New message";

        int actualIndex = _users100!.AddMessageToHistory(newUser, newMessage);

        Assert.AreEqual(expectedIndex, actualIndex);
        Assert.AreEqual(newUser, _users100![actualIndex].Name);
        Assert.AreEqual(newMessage, _users100![actualIndex].ChatHistory.Last().Content);
    }

    [TestMethod]
    public void AddMessageToHistoryWithFullListFromExistingUser()
    {
        int expectedIndex = 44;
        string userName = _users100![expectedIndex].Name;
        string newMessage = "New message";

        int actualIndex = _users100!.AddMessageToHistory(userName, newMessage);

        Assert.AreEqual(expectedIndex, actualIndex);
        Assert.AreEqual(userName, _users100![actualIndex].Name);
        Assert.AreEqual(newMessage, _users100![actualIndex].ChatHistory.Last().Content);
    }
}
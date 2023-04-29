using GPTwitchBot.GPT;

namespace GPTwitchBot;

public class Users
{
    List<User> _userList;
    public Users()
    {
        _userList = new List<User>();
    }

    public Users(IEnumerable<User> users)
    {
        _userList = users.ToList();
    }

    public User this[int index]
    {
        get => _userList[index];
    }

    public int AddMessageToHistory(string messageSender, string message)
    {
        DateTime messageTime = DateTime.Now;
        int index = FindOrCreateUser(messageSender);
        _userList[index].Name = messageSender;
        _userList[index].LastMessageDate = messageTime;
        _userList[index].ChatHistory.Add(new Message() { Role = "user", Content = message });
        return index;
    }

    private int FindOrCreateUser(string userName)
    {
        int index = _userList.FindIndex(u => u.Name == userName);
        if (index == -1)
        {
            User newUser = new User();
            if (_userList.Count <= 100)
            {
                _userList.Add(newUser);
                index = _userList.Count - 1;
            }
            else
            {
                index = _userList.FindIndex(u => u == _userList.Min());
                _userList[index] = newUser;
            }
        }
        return index;
    }
}
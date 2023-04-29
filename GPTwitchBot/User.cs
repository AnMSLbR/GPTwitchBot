using GPTwitchBot.GPT;

namespace GPTwitchBot
{
    public class User : IComparable<User>
    {
        public string Name { get; set; } = string.Empty;
        public DateTime LastMessageDate { get; set; }
        public List<Message> ChatHistory { get; set; }

        public User()
        {
            ChatHistory = new List<Message>();
        }

        public int CompareTo(User? other)
        {
            return LastMessageDate.CompareTo(other?.LastMessageDate);
        }
    }
}

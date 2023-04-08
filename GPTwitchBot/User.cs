using GPTwitchBot.GPT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTwitchBot
{
    internal class User : IComparable<User>
    {
        public string Name { get; set; }
        public DateTime LastMessageDate { get; set; }
        public List<Message> ChatHistory { get; set; }

        public int CompareTo(User other)
        {
            return LastMessageDate.CompareTo(other.LastMessageDate);
        }
    }
}

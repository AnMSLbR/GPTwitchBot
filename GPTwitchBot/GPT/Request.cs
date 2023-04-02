using System.Text.Json.Serialization;

namespace GPTwitchBot.GPT
{
    internal class Request
    {
        [JsonPropertyName("model")]
        public string ModelId { get; set; } = "";
        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; } = new();
    }
}

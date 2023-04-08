using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;


namespace GPTwitchBot.GPT
{
    internal class GPTClient
    {
        string _apiKey = "api_Key";
        string _endpoint = "https://api.openai.com/v1/chat/completions";
       // List<Message> _messages;
        HttpClient _httpClient;

       //public List<Message> Messages { get => _messages; set => _messages = value; }

        public GPTClient()
        {
           // _messages = new List<Message>();
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> ChatAsync(List<Message> messages)
        {
            var requestData = new Request() { ModelId = "gpt-3.5-turbo", Messages = messages };

            using var response = await _httpClient.PostAsJsonAsync(_endpoint, requestData);
            ResponseData? responseData = await response.Content.ReadFromJsonAsync<ResponseData>();
            var choices = responseData?.Choices ?? new List<Choice>();
            if (choices.Count == 0)
            {
                return "No choices were returned by the API";
            }
            var choice = choices[0];
            var responseMessage = choice.Message;
            messages.Add(responseMessage);
            return responseMessage.Content.Trim();      
        }

    }
}

using System.Net.Http.Json;

namespace GPTwitchBot.GPT
{
    public class GPTClient
    {
        string _apiKey = string.Empty;
        string _endpoint = string.Empty;
        HttpClient? _httpClient;

        public void Initialize(string? apiKey, string? endpoint)
        {
            if (apiKey is not null && endpoint is not null)
            {
                _httpClient = new HttpClient();
                _apiKey = apiKey;
                _endpoint = endpoint;
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");    
            }
        }

        public async Task<string> ChatAsync(List<Message> messages)
        {
            var requestData = new Request() { ModelId = "gpt-3.5-turbo", Messages = messages };

            using var response = await _httpClient!.PostAsJsonAsync(_endpoint, requestData);
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

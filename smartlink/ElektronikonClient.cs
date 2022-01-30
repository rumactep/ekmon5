using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace smartlink;

public interface IElektronikonClient {
    Task<string> AskAsync(string questionsString);
}

public class ElektronikonClient : IElektronikonClient {
    private readonly string _url;
    private readonly HttpClient _client;

    public ElektronikonClient(string url) {
        _url = url;
        _client = new HttpClient();
    }

    public async Task<string> AskAsync(string questionsString) {
        var urlParameters = new Dictionary<string, string> {
            { "QUESTION", questionsString }
        };
        var parametersContent = new FormUrlEncodedContent(urlParameters);
        var response = await _client.PostAsync(_url, parametersContent);
        string answer = await response.Content.ReadAsStringAsync();
        return answer;
    }
}
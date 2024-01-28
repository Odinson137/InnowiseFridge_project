using System.Net.Http.Headers;

namespace InnowiseFridgeClient.Services;

public class HttpClientService
{
    private readonly HttpClient _client;
    public HttpClientService(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri("http://localhost:5145");
    }

    public void Authentificating(string jwtToken)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
    }
    
    public HttpClient GetClient() => _client;
}
using System.Text;
using System.Text.Json;
using Contracts.External.AtProto;
using Microsoft.Extensions.Configuration;
using Models.External.AtProto.Server;

namespace Services.External.AtProto
{
    public class AtProtoServerService : IAtProtoServerService
    {
        private readonly HttpClient _httpClient;
        private static string _accessToken;
        private static DateTime _accessTokenExpiration;
        private static string _identifier;
        private static string _password;

        public AtProtoServerService(HttpClient httpClient, IConfiguration configuration)
        {
            httpClient.BaseAddress = new Uri(configuration["AtProto:BaseUrl"] + configuration["AtProto:Server"]);
            _httpClient = httpClient;
            _identifier = configuration["AtProto:Auth:Identifier"];
            _password = configuration["AtProto:Auth:Password"];
        }

        private async Task CreateSession()
        {
            CreateSessionRequest request = new() { Identifier = _identifier, Password = _password };
            string json = JsonSerializer.Serialize(request);

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "createSession", new StringContent(json, Encoding.UTF8, "application/json"));
            string responseJson = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            CreateSessionResponse createSessionResponse = JsonSerializer.Deserialize<CreateSessionResponse>(responseJson);
            _accessToken = createSessionResponse?.AccessJwt;
            _accessTokenExpiration = DateTime.Now.AddHours(1);
        }

        public async Task<string> GetAccessToken()
        {
            if (string.IsNullOrEmpty(_accessToken) || DateTime.Now >= _accessTokenExpiration)
                await CreateSession();

            return _accessToken;
        }
    }
}
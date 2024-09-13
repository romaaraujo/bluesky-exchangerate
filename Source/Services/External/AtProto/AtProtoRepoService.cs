using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Contracts.External.AtProto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models.External.AtProto.Repo;

namespace Services.External.AtProto
{
    public class AtProtoRepoService : IAtProtoRepoService
    {
        private readonly ILogger<AtProtoRepoService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IAtProtoServerService _atProtoServerService;
        private readonly string _repoName;
        private readonly string _repoCollection;

        public AtProtoRepoService(ILogger<AtProtoRepoService> logger, HttpClient httpClient, IConfiguration configuration, IAtProtoServerService atProtoServerService)
        {
            _logger = logger;
            httpClient.BaseAddress = new Uri(configuration["AtProto:BaseUrl"] + configuration["AtProto:Repo"]);
            _httpClient = httpClient;
            _atProtoServerService = atProtoServerService;
            _repoName = configuration["AtProto:RepoName"];
            _repoCollection = configuration["AtProto:RepoCollection"];
        }

        public async Task CreateRecord(string message)
        {
            string json = JsonSerializer.Serialize(new CreateRecordRequest { Record = new() { Text = message, CreatedAt = DateTime.Now }, Collection = _repoCollection, Repo = _repoName });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _atProtoServerService.GetAccessToken());

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "createRecord", new StringContent(json, Encoding.UTF8, "application/json"));
            string responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                _logger.LogError("Failed to create record: {responseJson}", responseJson);
            else
                _logger.LogInformation("Record created: {responseJson}", responseJson);
        }

    }
}
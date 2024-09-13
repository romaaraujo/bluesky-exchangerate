using System.Text.Json;
using Contracts;
using Microsoft.Extensions.Configuration;
using Models.External;
using Models.Internal;

namespace Services
{
    public class ExchangeRateApiCurrencyConvertorService : ICurrencyConvertorService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;


        public ExchangeRateApiCurrencyConvertorService(HttpClient httpClient, IConfiguration configuration)
        {
            httpClient.Timeout = TimeSpan.FromSeconds(15);
            _httpClient = httpClient;
            _baseUrl = configuration["ExchangeRateApi:BaseUrl"];
            _apiKey = configuration["ExchangeRateApi:ApiKey"];
        }

        public async Task<CurrencyConvert> Convert(CurrencyConvert currencyConvert)
        {
            string url = _baseUrl.Replace("{ApiKey}", _apiKey).Replace("{Currency}", currencyConvert.FromCurrency.ToString());
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            string data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to convert currency. Status code: {response.StatusCode}");
            }

            ExchangeRateApiResponse exchangeRateApiResponse = JsonSerializer.Deserialize<ExchangeRateApiResponse>(data);

            if (exchangeRateApiResponse?.Result != "success")
            {
                throw new Exception("Failed to convert currency. Invalid response.");
            }
            else if (exchangeRateApiResponse.ConversionRates is not null && exchangeRateApiResponse.ConversionRates.TryGetValue(currencyConvert.ToCurrency.ToString(), out decimal rate))
            {
                currencyConvert.ExchangeRate = currencyConvert.Amount * rate;
            }

            return currencyConvert;
        }
    }
}
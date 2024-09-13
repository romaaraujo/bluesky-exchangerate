using Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models.Internal;

namespace Services
{
    public class CurrencyConvertorHostedService : BackgroundService
    {
        private readonly ILogger<CurrencyConvertorHostedService> _logger;
        private readonly ICurrencyConvertorService _currencyConvertorService;
        private readonly int _intervalSeconds = 1800;
        private static HashSet<CurrencyConvert> _currenciesForConversion = new() { };

        public CurrencyConvertorHostedService(ILogger<CurrencyConvertorHostedService> logger, ICurrencyConvertorService currencyConvertorService)
        {
            _logger = logger;
            _currencyConvertorService = currencyConvertorService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CurrencyConvertorBackgroundService is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (CurrencyConvert currency in _currenciesForConversion)
                {
                    DoWork(currency);
                }
                await Task.Delay(TimeSpan.FromSeconds(_intervalSeconds), stoppingToken);
            }
        }

        private async void DoWork(CurrencyConvert currencyConvert)
        {
            try
            {
                await _currencyConvertorService.Convert(currencyConvert);
                _logger.LogInformation("Converted {fromCurrency} {amount} to {toCurrency} {convertedAmount} (ExchangeRate: {ExchangeRate})", currencyConvert.FromCurrency, currencyConvert.Amount, currencyConvert.ToCurrency, currencyConvert.GetCalculateAmount(), currencyConvert.ExchangeRate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while converting currency: {errorMessage}", ex.Message);
            }
        }

        public static void SetCurrenciesForConversion(HashSet<CurrencyConvert> currenciesForConversion)
        {
            _currenciesForConversion = currenciesForConversion;
        }
    }
}
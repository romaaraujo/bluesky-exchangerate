using Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Contracts;
using Enums;
using Models.Internal;
using Contracts.External.AtProto;
using Services.External.AtProto;

namespace BlueSkyExchangeRate;

public class Program
{
    public static void Main(string[] args)
    {
        using (var scope = CreateHostBuilder(args).Build().Services.CreateScope())
        {
            var atProtoRepoService = scope.ServiceProvider.GetRequiredService<IAtProtoRepoService>();

            var UsdToBrl = new CurrencyConvert(Currencies.USD, Currencies.BRL, 1);
            UsdToBrl.ExchangeRateChanged += (sender, args) => atProtoRepoService.CreateRecord(
                $"{UsdToBrl.FromCurrency} {UsdToBrl.Amount} equivale a {UsdToBrl.ToCurrency} {UsdToBrl.GetCalculateAmount()} ({DateTime.Now:HH:mm})");

            HashSet<CurrencyConvert> currenciesForConversion = new() { UsdToBrl };

            CurrencyConvertorHostedService.SetCurrenciesForConversion(currenciesForConversion);
        }

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IAtProtoServerService, AtProtoServerService>();
                services.AddSingleton<IAtProtoRepoService, AtProtoRepoService>();
                services.AddSingleton<ICurrencyConvertorService, ExchangeRateApiCurrencyConvertorService>();
                services.AddHostedService<CurrencyConvertorHostedService>();

                services.AddHttpClient();
            });
}

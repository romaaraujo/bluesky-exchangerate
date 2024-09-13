using Enums;
using Models.Internal;

namespace Contracts
{
    public interface ICurrencyConvertorService
    {
        Task<CurrencyConvert> Convert(CurrencyConvert currencyConvert);
    }
}
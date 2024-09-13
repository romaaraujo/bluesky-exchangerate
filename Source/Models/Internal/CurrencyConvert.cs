using Enums;

namespace Models.Internal
{
    public class CurrencyConvert
    {
        public CurrencyConvert(Currencies fromCurrency, Currencies toCurrency, decimal amount)
        {
            FromCurrency = fromCurrency;
            ToCurrency = toCurrency;
            Amount = amount;
        }

        public Currencies FromCurrency { get; private set; }
        public Currencies ToCurrency { get; private set; }
        public decimal Amount { get; private set; }
        private decimal _exchangeRate;

        public decimal ExchangeRate
        {
            get => _exchangeRate;
            set
            {
                if (_exchangeRate != value)
                {
                    _exchangeRate = value;
                    OnExchangeRateChanged();
                }
            }
        }

        public EventHandler ExchangeRateChanged;

        public void OnExchangeRateChanged()
        {
            ExchangeRateChanged?.Invoke(this, new());
        }

        public decimal GetCalculateAmount() => Amount * ExchangeRate;
    }
}
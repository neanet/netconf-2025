using BudgetApp.Models;

namespace BudgetApp.Services;

/// <summary>
/// Implementation of ICurrencyService for currency conversion operations.
/// </summary>
public class CurrencyService : ICurrencyService
{
    private decimal? _exchangeRate;

    public decimal? GetExchangeRate()
    {
        return _exchangeRate;
    }

    public Task SetExchangeRateAsync(decimal pesosPerDollar)
    {
        if (!ValidateExchangeRate(pesosPerDollar))
        {
            throw new ArgumentException($"La tasa de cambio debe estar entre 0.001 y 10,000. Valor recibido: {pesosPerDollar}");
        }

        _exchangeRate = pesosPerDollar;
        return Task.CompletedTask;
    }

    public decimal ConvertToDollars(decimal pesosAmount)
    {
        if (!_exchangeRate.HasValue)
        {
            throw new InvalidOperationException("La tasa de cambio no está configurada. Por favor, configure la tasa de cambio antes de convertir.");
        }

        var converted = pesosAmount / _exchangeRate.Value;
        return Math.Round(converted, 2, MidpointRounding.ToEven);
    }

    public decimal ConvertToPesos(decimal dollarsAmount)
    {
        if (!_exchangeRate.HasValue)
        {
            throw new InvalidOperationException("La tasa de cambio no está configurada. Por favor, configure la tasa de cambio antes de convertir.");
        }

        var converted = dollarsAmount * _exchangeRate.Value;
        return Math.Round(converted, 2, MidpointRounding.ToEven);
    }

    public decimal ConvertCurrency(decimal amount, Currency fromCurrency, Currency toCurrency)
    {
        if (fromCurrency == toCurrency)
        {
            return amount;
        }

        if (!_exchangeRate.HasValue)
        {
            throw new InvalidOperationException("La tasa de cambio no está configurada. Por favor, configure la tasa de cambio antes de convertir.");
        }

        return fromCurrency == Currency.Pesos
            ? ConvertToDollars(amount)
            : ConvertToPesos(amount);
    }

    public bool ValidateExchangeRate(decimal pesosPerDollar)
    {
        const decimal minRate = 0.001m;
        const decimal maxRate = 10000m;
        return pesosPerDollar >= minRate && pesosPerDollar <= maxRate;
    }

    public bool IsExchangeRateConfigured()
    {
        return _exchangeRate.HasValue;
    }
}


// Service Contract: ICurrencyService
// Purpose: Business logic for currency conversion operations
// Feature: 001-currency-conversion

using BudgetApp.Models;

namespace BudgetApp.Services;

/// <summary>
/// Service interface for currency conversion operations including
/// exchange rate configuration and currency conversion calculations.
/// </summary>
public interface ICurrencyService
{
    /// <summary>
    /// Gets the current exchange rate (Pesos per Dollar).
    /// </summary>
    /// <returns>Exchange rate, or null if not configured</returns>
    decimal? GetExchangeRate();

    /// <summary>
    /// Sets the exchange rate for currency conversions.
    /// </summary>
    /// <param name="pesosPerDollar">Exchange rate (e.g., 50.0 means 1 USD = 50 ARS)</param>
    /// <exception cref="ArgumentException">Thrown if exchange rate is invalid (≤ 0 or out of range)</exception>
    Task SetExchangeRateAsync(decimal pesosPerDollar);

    /// <summary>
    /// Converts a value from Pesos to Dollars using the configured exchange rate.
    /// </summary>
    /// <param name="pesosAmount">Amount in Pesos</param>
    /// <returns>Converted amount in Dollars, rounded to 2 decimal places</returns>
    /// <exception cref="InvalidOperationException">Thrown if exchange rate is not configured</exception>
    decimal ConvertToDollars(decimal pesosAmount);

    /// <summary>
    /// Converts a value from Dollars to Pesos using the configured exchange rate.
    /// </summary>
    /// <param name="dollarsAmount">Amount in Dollars</param>
    /// <returns>Converted amount in Pesos, rounded to 2 decimal places</returns>
    /// <exception cref="InvalidOperationException">Thrown if exchange rate is not configured</exception>
    decimal ConvertToPesos(decimal dollarsAmount);

    /// <summary>
    /// Converts a value from one currency to another.
    /// </summary>
    /// <param name="amount">Amount to convert</param>
    /// <param name="fromCurrency">Source currency</param>
    /// <param name="toCurrency">Target currency</param>
    /// <returns>Converted amount, rounded to 2 decimal places</returns>
    /// <exception cref="InvalidOperationException">Thrown if exchange rate is not configured or currencies are the same</exception>
    decimal ConvertCurrency(decimal amount, Currency fromCurrency, Currency toCurrency);

    /// <summary>
    /// Validates that an exchange rate is within acceptable range.
    /// </summary>
    /// <param name="pesosPerDollar">Exchange rate to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    bool ValidateExchangeRate(decimal pesosPerDollar);

    /// <summary>
    /// Checks if an exchange rate is currently configured.
    /// </summary>
    /// <returns>True if exchange rate is configured, false otherwise</returns>
    bool IsExchangeRateConfigured();
}


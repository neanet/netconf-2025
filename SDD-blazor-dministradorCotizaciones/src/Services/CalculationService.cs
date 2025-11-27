using BudgetApp.Models;

namespace BudgetApp.Services;

/// <summary>
/// Implementation of ICalculationService for budget calculations.
/// </summary>
public class CalculationService : ICalculationService
{
    public decimal CalculateSubtotal(decimal quantity, decimal unitPrice)
    {
        var subtotal = quantity * unitPrice;
        return Math.Round(subtotal, 2, MidpointRounding.ToEven);
    }

    public decimal CalculateTotal(IEnumerable<BudgetItem> items)
    {
        var total = items.Sum(item => item.Subtotal);
        return Math.Round(total, 2, MidpointRounding.ToEven);
    }

    public string FormatCurrency(decimal amount)
    {
        // Format with 2 decimal places and thousand separators
        // Using InvariantCulture ensures consistent formatting (1,234.56)
        return amount.ToString("N2", System.Globalization.CultureInfo.InvariantCulture);
    }

    public string FormatCurrency(decimal amount, Currency currency)
    {
        // Format with 2 decimal places and thousand separators
        var formatted = FormatCurrency(amount);
        
        // Add currency code prefix
        var code = currency == Currency.Pesos ? "ARS" : "USD";
        return $"{code} ${formatted}";
    }

    public bool ValidateCalculation(decimal amount)
    {
        // Check reasonable bounds (prevent unrealistic values)
        // decimal type doesn't support Infinity or NaN, so we only check bounds
        const decimal maxValue = 999_999_999.99m;
        const decimal minValue = -999_999_999.99m;

        return amount >= minValue && amount <= maxValue;
    }
}


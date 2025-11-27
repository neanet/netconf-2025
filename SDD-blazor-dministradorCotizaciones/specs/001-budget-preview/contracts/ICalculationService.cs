// Service Contract: ICalculationService
// Purpose: Business logic for budget calculations
// Feature: 001-budget-preview

namespace BudgetApp.Services;

/// <summary>
/// Service interface for budget calculation operations including
/// subtotal and total calculations with proper rounding.
/// </summary>
public interface ICalculationService
{
    /// <summary>
    /// Calculates the subtotal for a budget item.
    /// </summary>
    /// <param name="quantity">Item quantity</param>
    /// <param name="unitPrice">Item unit price</param>
    /// <returns>Subtotal rounded to 2 decimal places</returns>
    decimal CalculateSubtotal(decimal quantity, decimal unitPrice);

    /// <summary>
    /// Calculates the total for a list of budget items.
    /// </summary>
    /// <param name="items">List of budget items</param>
    /// <returns>Total amount rounded to 2 decimal places</returns>
    decimal CalculateTotal(IEnumerable<BudgetItem> items);

    /// <summary>
    /// Formats a monetary value for display with thousand separators and 2 decimal places.
    /// </summary>
    /// <param name="amount">Amount to format</param>
    /// <returns>Formatted string (e.g., "1,234.56")</returns>
    string FormatCurrency(decimal amount);

    /// <summary>
    /// Validates that a calculation result is within acceptable bounds.
    /// </summary>
    /// <param name="amount">Amount to validate</param>
    /// <returns>True if valid, false if overflow or invalid</returns>
    bool ValidateCalculation(decimal amount);
}


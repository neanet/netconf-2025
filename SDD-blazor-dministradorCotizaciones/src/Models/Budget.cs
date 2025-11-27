using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Models;

/// <summary>
/// Represents a complete budget document that can be previewed, saved, and loaded from LocalStorage.
/// </summary>
public record Budget
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required(ErrorMessage = "La información del cliente es requerida")]
    public Client Client { get; init; } = null!;

    public List<BudgetItem> Items { get; init; } = new();

    /// <summary>
    /// Currency used for this budget. Defaults to Pesos.
    /// </summary>
    public Currency Currency { get; init; } = Currency.Pesos;

    public DateTime CreatedDate { get; init; } = DateTime.UtcNow;

    public DateTime LastModifiedDate { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Calculated total (sum of all item subtotals). Read-only property.
    /// </summary>
    public decimal Total => Items.Sum(item => item.Subtotal);
}


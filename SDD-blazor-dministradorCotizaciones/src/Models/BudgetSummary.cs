namespace BudgetApp.Models;

/// <summary>
/// Represents a summary view of a budget for listing purposes (used in budget list view).
/// </summary>
public record BudgetSummary
{
    public Guid Id { get; init; }

    public string ClientName { get; init; } = string.Empty;

    public int ItemCount { get; init; }

    public decimal Total { get; init; }

    public Currency Currency { get; init; } = Currency.Pesos;

    public DateTime CreatedDate { get; init; }

    public DateTime LastModifiedDate { get; init; }
}


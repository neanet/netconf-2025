using BudgetApp.Models;

namespace BudgetApp.Services;

/// <summary>
/// Service interface for budget management operations including
/// creation, retrieval, update, and persistence.
/// </summary>
public interface IBudgetService
{
    /// <summary>
    /// Creates a new budget with the specified client information.
    /// </summary>
    /// <param name="client">Client information for the budget</param>
    /// <returns>Newly created budget with generated ID and timestamps</returns>
    Task<Budget> CreateBudgetAsync(Client client);

    /// <summary>
    /// Retrieves a budget by its unique identifier.
    /// </summary>
    /// <param name="budgetId">Unique identifier of the budget</param>
    /// <returns>Budget if found, null otherwise</returns>
    Task<Budget?> GetBudgetAsync(Guid budgetId);

    /// <summary>
    /// Retrieves all saved budgets as summaries for listing.
    /// </summary>
    /// <returns>List of budget summaries</returns>
    Task<List<BudgetSummary>> GetAllBudgetsAsync();

    /// <summary>
    /// Updates an existing budget.
    /// </summary>
    /// <param name="budget">Budget with updated information</param>
    /// <returns>Updated budget</returns>
    /// <exception cref="ArgumentException">Thrown if budget ID not found</exception>
    Task<Budget> UpdateBudgetAsync(Budget budget);

    /// <summary>
    /// Deletes a budget by its unique identifier.
    /// </summary>
    /// <param name="budgetId">Unique identifier of the budget to delete</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteBudgetAsync(Guid budgetId);

    /// <summary>
    /// Calculates the total for a budget based on its items.
    /// </summary>
    /// <param name="budget">Budget to calculate total for</param>
    /// <returns>Calculated total amount</returns>
    decimal CalculateTotal(Budget budget);

    /// <summary>
    /// Saves a budget to LocalStorage (creates or updates).
    /// </summary>
    /// <param name="budget">Budget to save</param>
    /// <returns>Saved budget</returns>
    Task<Budget> SaveBudgetAsync(Budget budget);
}


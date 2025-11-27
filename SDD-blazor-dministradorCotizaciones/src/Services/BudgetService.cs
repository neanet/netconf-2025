using BudgetApp.Models;
using System.Collections.Concurrent;

namespace BudgetApp.Services;

/// <summary>
/// Implementation of IBudgetService for budget management operations.
/// </summary>
public class BudgetService : IBudgetService
{
    private readonly ILocalStorageService _localStorage;
    private readonly ICalculationService _calculationService;
    private const string StorageKey = "budgets";

    public BudgetService(ILocalStorageService localStorage, ICalculationService calculationService)
    {
        _localStorage = localStorage;
        _calculationService = calculationService;
    }

    public async Task<Budget> CreateBudgetAsync(Client client)
    {
        // Create budget in memory only - don't save until user explicitly saves
        var budget = new Budget
        {
            Id = Guid.NewGuid(),
            Client = client,
            Items = new List<BudgetItem>(),
            Currency = Currency.Pesos, // Default currency
            CreatedDate = DateTime.UtcNow,
            LastModifiedDate = DateTime.UtcNow
        };

        // Don't save automatically - let user save explicitly
        return budget;
    }

    public async Task<Budget?> GetBudgetAsync(Guid budgetId)
    {
        try
        {
            // Load full budget data
            var allBudgets = await LoadAllBudgetsAsync();
            return allBudgets.FirstOrDefault(b => b.Id == budgetId);
        }
        catch (Exception ex) when (ex.Message.Contains("corrupt") || ex.Message.Contains("invalid"))
        {
            // Handle corrupted data - return null and let caller handle
            return null;
        }
    }

    public async Task<List<BudgetSummary>> GetAllBudgetsAsync()
    {
        var budgets = await LoadAllBudgetsAsync();
        return budgets.Select(b => new BudgetSummary
        {
            Id = b.Id,
            ClientName = b.Client.Name,
            ItemCount = b.Items.Count,
            Total = b.Total,
            Currency = b.Currency,
            CreatedDate = b.CreatedDate,
            LastModifiedDate = b.LastModifiedDate
        }).ToList();
    }

    public async Task<Budget> UpdateBudgetAsync(Budget budget)
    {
        var budgets = await LoadAllBudgetsAsync();
        var existingIndex = budgets.FindIndex(b => b.Id == budget.Id);

        if (existingIndex == -1)
        {
            throw new ArgumentException($"Presupuesto con ID {budget.Id} no encontrado.");
        }

        var updatedBudget = budget with { LastModifiedDate = DateTime.UtcNow };
        budgets[existingIndex] = updatedBudget;

        await SaveAllBudgetsAsync(budgets);
        return updatedBudget;
    }

    public async Task<bool> DeleteBudgetAsync(Guid budgetId)
    {
        var budgets = await LoadAllBudgetsAsync();
        var budgetToRemove = budgets.FirstOrDefault(b => b.Id == budgetId);

        if (budgetToRemove == null)
        {
            return false;
        }

        budgets.Remove(budgetToRemove);
        await SaveAllBudgetsAsync(budgets);
        return true;
    }

    public decimal CalculateTotal(Budget budget)
    {
        return _calculationService.CalculateTotal(budget.Items);
    }

    public async Task<Budget> SaveBudgetAsync(Budget budget)
    {
        // Ensure client name is not empty - use placeholder if needed
        var client = budget.Client;
        if (string.IsNullOrWhiteSpace(client.Name))
        {
            client = client with { Name = "Sin nombre" };
        }

        var updatedBudget = budget with 
        { 
            Client = client,
            LastModifiedDate = DateTime.UtcNow 
        };
        await SaveBudgetInternalAsync(updatedBudget);
        return updatedBudget;
    }

    private async Task<List<Budget>> LoadAllBudgetsAsync()
    {
        try
        {
            var budgets = await _localStorage.GetItemAsync<List<Budget>>(StorageKey);
            return budgets ?? new List<Budget>();
        }
        catch (System.Text.Json.JsonException)
        {
            // Handle corrupted JSON data
            throw new InvalidOperationException("Los datos en LocalStorage están corruptos. Por favor, limpie el almacenamiento local.");
        }
        catch (Exception)
        {
            // Log error and return empty list to prevent app crash
            return new List<Budget>();
        }
    }

    private async Task SaveAllBudgetsAsync(List<Budget> budgets)
    {
        await _localStorage.SetItemAsync(StorageKey, budgets);
    }

    private async Task SaveBudgetInternalAsync(Budget budget)
    {
        var budgets = await LoadAllBudgetsAsync();
        var existingIndex = budgets.FindIndex(b => b.Id == budget.Id);

        if (existingIndex >= 0)
        {
            budgets[existingIndex] = budget;
        }
        else
        {
            budgets.Add(budget);
        }

        try
        {
            await SaveAllBudgetsAsync(budgets);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("lleno") || ex.Message.Contains("full"))
        {
            throw new InvalidOperationException("LocalStorage está lleno. Por favor, elimine algunos presupuestos antiguos.", ex);
        }
    }
}


using BudgetApp.Models;

namespace BudgetApp.Services;

/// <summary>
/// Service for handling auto-save functionality with debounce.
/// </summary>
public class AutoSaveService
{
    private readonly IBudgetService _budgetService;
    private System.Threading.Timer? _debounceTimer;
    private Budget? _pendingBudget;
    private readonly object _lock = new object();

    public AutoSaveService(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    /// <summary>
    /// Schedules an auto-save with debounce (500ms delay).
    /// </summary>
    /// <param name="budget">Budget to save</param>
    public void ScheduleAutoSave(Budget budget)
    {
        lock (_lock)
        {
            _pendingBudget = budget;

            _debounceTimer?.Dispose();
            _debounceTimer = new System.Threading.Timer(async _ =>
            {
                Budget? budgetToSave;
                lock (_lock)
                {
                    budgetToSave = _pendingBudget;
                    _pendingBudget = null;
                }

                if (budgetToSave != null)
                {
                    try
                    {
                        await _budgetService.SaveBudgetAsync(budgetToSave);
                    }
                    catch
                    {
                        // Silently fail for auto-save - user can manually save if needed
                    }
                }
            }, null, TimeSpan.FromMilliseconds(500), Timeout.InfiniteTimeSpan);
        }
    }

    /// <summary>
    /// Cancels any pending auto-save.
    /// </summary>
    public void CancelAutoSave()
    {
        lock (_lock)
        {
            _debounceTimer?.Dispose();
            _debounceTimer = null;
            _pendingBudget = null;
        }
    }
}


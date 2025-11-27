// Service Contract: ILocalStorageService
// Purpose: Abstraction for LocalStorage operations
// Feature: 001-budget-preview

namespace BudgetApp.Services;

/// <summary>
/// Service interface for LocalStorage operations, abstracting
/// browser LocalStorage access to enable testing and future migration.
/// </summary>
public interface ILocalStorageService
{
    /// <summary>
    /// Retrieves a value from LocalStorage by key.
    /// </summary>
    /// <typeparam name="T">Type of the value to retrieve</typeparam>
    /// <param name="key">Storage key</param>
    /// <returns>Deserialized value if found, default(T) otherwise</returns>
    Task<T?> GetItemAsync<T>(string key);

    /// <summary>
    /// Stores a value in LocalStorage with the specified key.
    /// </summary>
    /// <typeparam name="T">Type of the value to store</typeparam>
    /// <param name="key">Storage key</param>
    /// <param name="value">Value to store (will be serialized to JSON)</param>
    /// <exception cref="InvalidOperationException">Thrown if LocalStorage is full or unavailable</exception>
    Task SetItemAsync<T>(string key, T value);

    /// <summary>
    /// Removes a value from LocalStorage by key.
    /// </summary>
    /// <param name="key">Storage key to remove</param>
    Task RemoveItemAsync(string key);

    /// <summary>
    /// Clears all data from LocalStorage.
    /// </summary>
    Task ClearAsync();

    /// <summary>
    /// Checks if LocalStorage is available in the current browser context.
    /// </summary>
    /// <returns>True if LocalStorage is available, false otherwise</returns>
    Task<bool> IsAvailableAsync();

    /// <summary>
    /// Gets the amount of storage space used (if available).
    /// </summary>
    /// <returns>Estimated bytes used, or null if not available</returns>
    Task<long?> GetStorageUsedAsync();
}


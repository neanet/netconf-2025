using Blazored.LocalStorage;

namespace BudgetApp.Services;

/// <summary>
/// Implementation of ILocalStorageService using Blazored.LocalStorage.
/// </summary>
public class LocalStorageService : ILocalStorageService
{
    private readonly Blazored.LocalStorage.ILocalStorageService _blazoredLocalStorage;

    public LocalStorageService(Blazored.LocalStorage.ILocalStorageService blazoredLocalStorage)
    {
        _blazoredLocalStorage = blazoredLocalStorage;
    }

    public async Task<T?> GetItemAsync<T>(string key)
    {
        try
        {
            if (await IsAvailableAsync())
            {
                return await _blazoredLocalStorage.GetItemAsync<T>(key);
            }
            return default(T);
        }
        catch
        {
            return default(T);
        }
    }

    public async Task SetItemAsync<T>(string key, T value)
    {
        if (!await IsAvailableAsync())
        {
            throw new InvalidOperationException("LocalStorage no está disponible en este navegador.");
        }

        try
        {
            await _blazoredLocalStorage.SetItemAsync(key, value);
        }
        catch (Exception ex) when (ex.Message.Contains("quota") || ex.Message.Contains("QUOTA") || ex.Message.Contains("exceeded"))
        {
            throw new InvalidOperationException("LocalStorage está lleno. Por favor, elimine algunos presupuestos antiguos o exporte datos.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error al guardar en LocalStorage: {ex.Message}", ex);
        }
    }

    public async Task RemoveItemAsync(string key)
    {
        if (await IsAvailableAsync())
        {
            await _blazoredLocalStorage.RemoveItemAsync(key);
        }
    }

    public async Task ClearAsync()
    {
        if (await IsAvailableAsync())
        {
            await _blazoredLocalStorage.ClearAsync();
        }
    }

    public Task<bool> IsAvailableAsync()
    {
        try
        {
            // Blazored.LocalStorage handles availability internally
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<long?> GetStorageUsedAsync()
    {
        // Blazored.LocalStorage doesn't provide this directly
        // Return null as it's optional functionality
        return Task.FromResult<long?>(null);
    }
}


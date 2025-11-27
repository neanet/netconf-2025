# Quick Start: Conversión de Moneda

**Feature**: 001-currency-conversion  
**Date**: 2025-01-27

## Overview

Esta guía rápida explica cómo implementar la conversión de moneda en el sistema de presupuestos. La feature permite a los usuarios seleccionar entre Pesos y Dólares, configurar una tasa de cambio, y ver todos los valores convertidos automáticamente.

## Architecture Summary

### New Components

1. **CurrencySelector.razor**: Componente para seleccionar moneda (Pesos/Dólares)
2. **ExchangeRateConfig.razor**: Componente para configurar tasa de cambio

### New Services

1. **ICurrencyService / CurrencyService**: Lógica de conversión de moneda
2. **ICalculationService** (extended): Formateo por moneda

### Model Changes

1. **Budget**: Agregar propiedad `Currency` (enum)
2. **Currency**: Nuevo enum con valores `Pesos`, `Dollars`

## Implementation Steps

### Step 1: Create Currency Enum

**File**: `src/Models/Currency.cs`

```csharp
namespace BudgetApp.Models;

public enum Currency
{
    Pesos,
    Dollars
}
```

### Step 2: Update Budget Model

**File**: `src/Models/Budget.cs`

Agregar propiedad:
```csharp
public Currency Currency { get; init; } = Currency.Pesos;
```

### Step 3: Create ICurrencyService

**File**: `src/Services/ICurrencyService.cs`

Ver contrato completo en `contracts/ICurrencyService.cs`

### Step 4: Implement CurrencyService

**File**: `src/Services/CurrencyService.cs`

```csharp
namespace BudgetApp.Services;

public class CurrencyService : ICurrencyService
{
    private decimal? _exchangeRate;

    public decimal? GetExchangeRate() => _exchangeRate;

    public Task SetExchangeRateAsync(decimal pesosPerDollar)
    {
        if (!ValidateExchangeRate(pesosPerDollar))
            throw new ArgumentException("Tasa de cambio inválida");

        _exchangeRate = pesosPerDollar;
        return Task.CompletedTask;
    }

    public decimal ConvertToDollars(decimal pesosAmount)
    {
        if (!_exchangeRate.HasValue)
            throw new InvalidOperationException("Tasa de cambio no configurada");

        var converted = pesosAmount / _exchangeRate.Value;
        return Math.Round(converted, 2, MidpointRounding.ToEven);
    }

    public decimal ConvertToPesos(decimal dollarsAmount)
    {
        if (!_exchangeRate.HasValue)
            throw new InvalidOperationException("Tasa de cambio no configurada");

        var converted = dollarsAmount * _exchangeRate.Value;
        return Math.Round(converted, 2, MidpointRounding.ToEven);
    }

    public decimal ConvertCurrency(decimal amount, Currency fromCurrency, Currency toCurrency)
    {
        if (fromCurrency == toCurrency)
            return amount;

        return fromCurrency == Currency.Pesos
            ? ConvertToDollars(amount)
            : ConvertToPesos(amount);
    }

    public bool ValidateExchangeRate(decimal pesosPerDollar)
    {
        return pesosPerDollar > 0.001m && pesosPerDollar <= 10000m;
    }

    public bool IsExchangeRateConfigured() => _exchangeRate.HasValue;
}
```

### Step 5: Extend CalculationService

**File**: `src/Services/CalculationService.cs`

Agregar método sobrecargado:
```csharp
public string FormatCurrency(decimal amount, Currency currency)
{
    var formatted = FormatCurrency(amount); // Base formatting
    var code = currency == Currency.Pesos ? "ARS" : "USD";
    return $"{code} ${formatted}";
}
```

### Step 6: Register Services

**File**: `Program.cs`

```csharp
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
```

### Step 7: Create CurrencySelector Component

**File**: `src/Components/CurrencySelector.razor`

```razor
@namespace BudgetApp.Components
@inject ICurrencyService CurrencyService

<select @bind="SelectedCurrency" @bind:event="onchange" class="form-select">
    <option value="@Currency.Pesos">Pesos (ARS)</option>
    <option value="@Currency.Dollars">Dólares (USD)</option>
</select>

@code {
    [Parameter, EditorRequired]
    public Currency SelectedCurrency { get; set; }

    [Parameter]
    public EventCallback<Currency> SelectedCurrencyChanged { get; set; }

    private async Task OnCurrencyChanged(ChangeEventArgs e)
    {
        if (Enum.TryParse<Currency>(e.Value?.ToString(), out var currency))
        {
            SelectedCurrency = currency;
            await SelectedCurrencyChanged.InvokeAsync(currency);
        }
    }
}
```

### Step 8: Create ExchangeRateConfig Component

**File**: `src/Components/ExchangeRateConfig.razor`

```razor
@namespace BudgetApp.Components
@inject ICurrencyService CurrencyService

<EditForm Model="@ExchangeRateModel" OnValidSubmit="HandleSubmit">
    <DataAnnotationsValidator />
    <div class="mb-3">
        <label class="form-label">Tasa de Cambio (1 USD = X Pesos)</label>
        <InputNumber @bind-Value="ExchangeRateModel.PesosPerDollar" class="form-control" />
        <ValidationMessage For="@(() => ExchangeRateModel.PesosPerDollar)" />
    </div>
    <button type="submit" class="btn btn-primary">Configurar Tasa</button>
</EditForm>

@code {
    public class ExchangeRateModel
    {
        [Range(0.001, 10000, ErrorMessage = "La tasa debe estar entre 0.001 y 10,000")]
        public decimal PesosPerDollar { get; set; }
    }

    private ExchangeRateModel ExchangeRateModel { get; set; } = new();

    private async Task HandleSubmit()
    {
        await CurrencyService.SetExchangeRateAsync(ExchangeRateModel.PesosPerDollar);
        // Show success message
    }
}
```

### Step 9: Update BudgetEditor

**File**: `src/Pages/BudgetEditor.razor`

Agregar:
- `CurrencySelector` para seleccionar moneda
- `ExchangeRateConfig` para configurar tasa
- Lógica para convertir valores al cambiar moneda
- Usar `FormatCurrency(amount, currency)` en visualización

### Step 10: Update Components to Use Currency Formatting

**Files**: `BudgetItemTable.razor`, `BudgetSummary.razor`, etc.

Cambiar:
```csharp
@CalculationService.FormatCurrency(item.UnitPrice)
```

Por:
```csharp
@CalculationService.FormatCurrency(item.UnitPrice, budget.Currency)
```

## Key Implementation Notes

### Currency Conversion Flow

1. Usuario selecciona moneda → `Budget.Currency` se actualiza
2. Sistema obtiene valores base (siempre en Pesos)
3. Si moneda es Dólares → convierte usando `ICurrencyService`
4. Si moneda es Pesos → muestra valores base directamente
5. Valores convertidos se muestran con formato por moneda

### Value Storage Strategy

- **Base Values**: Siempre en Pesos (en `BudgetItem.UnitPrice`)
- **Display Values**: Convertidos on-the-fly según `Budget.Currency`
- **Persistence**: Solo `Budget.Currency` se persiste, no valores convertidos

### Exchange Rate Management

- **Session-based**: Tasa se configura en sesión actual
- **Validation**: Rango 0.001 a 10,000
- **Required**: Debe configurarse antes de cambiar moneda

## Testing Checklist

- [ ] Currency enum serializa/deserializa correctamente
- [ ] Budget con Currency se persiste y carga correctamente
- [ ] CurrencyService convierte valores correctamente
- [ ] ExchangeRateConfig valida entrada correctamente
- [ ] CurrencySelector actualiza Budget.Currency
- [ ] FormatCurrency muestra código de moneda correcto
- [ ] Conversión bidireccional mantiene precisión
- [ ] Presupuestos sin Currency usan default Pesos

## Next Steps

1. Implementar componentes UI
2. Integrar en BudgetEditor
3. Actualizar componentes de visualización
4. Escribir tests de componentes
5. Escribir tests de integración


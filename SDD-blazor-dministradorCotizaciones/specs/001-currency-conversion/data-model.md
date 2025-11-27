# Data Model: Conversión de Moneda

**Feature**: 001-currency-conversion  
**Date**: 2025-01-27

## Overview

Este documento describe los cambios al modelo de datos para soportar conversión de moneda en presupuestos. Los cambios son mínimos y se integran con el modelo existente sin romper compatibilidad.

## Entity Changes

### Budget (Modified)

**File**: `src/Models/Budget.cs`

**Changes**:
- Agregar propiedad `Currency` de tipo `Currency` (enum)
- Valor por defecto: `Currency.Pesos`
- Persistido con el presupuesto en LocalStorage

**Updated Definition**:
```csharp
public record Budget
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [Required(ErrorMessage = "La información del cliente es requerida")]
    public Client Client { get; init; } = null!;
    
    public List<BudgetItem> Items { get; init; } = new();
    
    // NEW: Currency property
    public Currency Currency { get; init; } = Currency.Pesos;
    
    public DateTime CreatedDate { get; init; } = DateTime.UtcNow;
    
    public DateTime LastModifiedDate { get; init; } = DateTime.UtcNow;
    
    public decimal Total => Items.Sum(item => item.Subtotal);
}
```

**Validation**:
- `Currency` es un enum, validación automática por tipo
- No requiere Data Annotations adicionales

**Serialization**:
- JSON serializa enum como string (`"Pesos"` o `"Dollars"`)
- Compatible con LocalStorage existente
- Backward compatible: presupuestos sin `Currency` usan default `Pesos`

---

### Currency (New)

**File**: `src/Models/Currency.cs`

**Purpose**: Enum para representar la moneda seleccionada en un presupuesto.

**Definition**:
```csharp
namespace BudgetApp.Models;

/// <summary>
/// Represents the currency used for a budget.
/// </summary>
public enum Currency
{
    /// <summary>
    /// Argentine Pesos (ARS)
    /// </summary>
    Pesos,
    
    /// <summary>
    /// US Dollars (USD)
    /// </summary>
    Dollars
}
```

**Usage**:
- Almacenado en `Budget.Currency`
- Usado en servicios de conversión
- Usado en componentes de UI para selección

**Extensibility**:
- Fácil agregar más monedas en el futuro (ej: `Euros`, `Reais`)
- Requiere actualizar lógica de conversión y formateo

---

### BudgetItem (No Changes)

**File**: `src/Models/BudgetItem.cs`

**Status**: ✅ **No changes required**

**Rationale**:
- `UnitPrice` siempre en moneda base (Pesos)
- Conversión se calcula on-the-fly al mostrar/calcular
- No necesita almacenar moneda individual

**Note**: Los valores en `BudgetItem` (Quantity, UnitPrice, Subtotal) siempre están en la moneda base (Pesos). La conversión se aplica al momento de mostrar o calcular totales según `Budget.Currency`.

---

### Client (No Changes)

**File**: `src/Models/Client.cs`

**Status**: ✅ **No changes required**

---

### BudgetSummary (No Changes)

**File**: `src/Models/BudgetSummary.cs`

**Status**: ✅ **No changes required**

**Note**: `BudgetSummary` muestra el total del presupuesto. El formateo por moneda se aplica en el componente de visualización, no en el modelo.

---

## Service Models

### ExchangeRate (Service-Level, Not Persisted)

**Purpose**: Representa la tasa de cambio configurada en la sesión actual.

**Location**: `ICurrencyService` (no es un modelo persistido)

**Conceptual Definition**:
```csharp
// Not a persisted model, but used in service layer
public record ExchangeRate
{
    public decimal PesosPerDollar { get; init; } // e.g., 50.0 means 1 USD = 50 ARS
    public DateTime LastUpdated { get; init; } = DateTime.UtcNow;
}
```

**Storage**:
- Almacenado en `ICurrencyService` durante la sesión
- Opcionalmente, puede persistirse en LocalStorage como configuración de usuario
- No se almacena en cada `Budget` (a menos que se requiera contexto histórico)

---

## Data Flow

### Creating a Budget

1. Usuario crea presupuesto → `Budget.Currency = Currency.Pesos` (default)
2. Usuario agrega items → `BudgetItem.UnitPrice` en Pesos
3. Usuario selecciona moneda → `Budget.Currency` se actualiza
4. Sistema recalcula valores convertidos para visualización
5. Al guardar → `Budget` con `Currency` se persiste en LocalStorage

### Loading a Budget

1. Sistema carga `Budget` desde LocalStorage
2. `Budget.Currency` se restaura (o default a `Pesos` si no existe)
3. Sistema muestra valores según `Budget.Currency`
4. Si cambia moneda → se actualiza `Budget.Currency` y se recalculan valores

### Converting Currency

1. Usuario cambia `Budget.Currency` de `Pesos` a `Dollars`
2. Sistema obtiene tasa de cambio desde `ICurrencyService`
3. Sistema recalcula todos los valores para visualización:
   - `UnitPrice` convertido = `UnitPrice` (Pesos) / `ExchangeRate`
   - `Subtotal` convertido = `Subtotal` (Pesos) / `ExchangeRate`
   - `Total` convertido = `Total` (Pesos) / `ExchangeRate`
4. Valores base en `BudgetItem` NO se modifican
5. `Budget.Currency` se actualiza
6. Al guardar → `Budget` con nueva `Currency` se persiste

---

## Migration Strategy

### Backward Compatibility

**Problem**: Presupuestos existentes no tienen propiedad `Currency`.

**Solution**: 
- Valor por defecto `Currency.Pesos` en el modelo
- Al cargar presupuestos sin `Currency`, se asume `Pesos`
- JSON deserialization maneja valores faltantes automáticamente

**Code**:
```csharp
// In BudgetService when loading
var budget = await _localStorage.GetItemAsync<Budget>(key);
// If Currency is missing in JSON, default value (Pesos) is used
```

### Versioning

- **Current Version**: No requiere versionado de esquema
- **Future Changes**: Si se agregan más monedas, el enum se extiende sin romper compatibilidad

---

## Validation Rules

### Currency

- **Type**: Enum (validación automática)
- **Required**: No (tiene valor por defecto)
- **Default**: `Currency.Pesos`

### Exchange Rate (Service-Level)

- **Range**: 0.001 a 10,000
- **Required**: Sí (para conversiones)
- **Validation**: `[Range(0.001, 10000, ErrorMessage = "La tasa de cambio debe estar entre 0.001 y 10,000")]`

---

## Storage Schema

### LocalStorage Key: `"budgets"`

**Before** (existing):
```json
[
  {
    "Id": "...",
    "Client": {...},
    "Items": [...],
    "CreatedDate": "...",
    "LastModifiedDate": "..."
  }
]
```

**After** (with currency):
```json
[
  {
    "Id": "...",
    "Client": {...},
    "Items": [...],
    "Currency": "Pesos",  // NEW
    "CreatedDate": "...",
    "LastModifiedDate": "..."
  }
]
```

**Compatibility**: ✅ Backward compatible - presupuestos sin `Currency` funcionan con default `Pesos`

---

## Summary

| Entity | Change Type | Impact |
|--------|-------------|--------|
| `Budget` | Modified | Agregar `Currency` property |
| `Currency` | New | Nuevo enum para monedas |
| `BudgetItem` | No change | Valores siempre en moneda base |
| `Client` | No change | - |
| `BudgetSummary` | No change | - |

**Migration Required**: ❌ No - Cambios son backward compatible  
**Breaking Changes**: ❌ No - Valores por defecto manejan compatibilidad


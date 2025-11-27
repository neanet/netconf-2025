# Quickstart: Vista Previa y Guardado/Carga de Presupuestos

**Feature**: 001-budget-preview  
**Date**: 2025-01-27

## Overview

This feature implements a professional budget preview display and complete save/load functionality using LocalStorage. The implementation follows Blazor WebAssembly best practices with modular components, service-based business logic, and Data Annotations validation.

## Architecture Summary

- **Components**: Modular Blazor components for preview, item table, client info, and budget list
- **Services**: Business logic separated into services (BudgetService, LocalStorageService, CalculationService)
- **Models**: Typed C# 13 records with Data Annotations validation
- **Storage**: LocalStorage via Blazored.LocalStorage abstraction

## Key Components

### BudgetPreview.razor
Main preview component that displays the complete budget in a professional format.

**Props**:
- `Budget Budget` (required): Budget to display
- `bool ShowClientInfo` (optional, default: true): Whether to show client information

**Usage**:
```razor
<BudgetPreview Budget="@currentBudget" />
```

### BudgetItemTable.razor
Displays budget items in a formatted table with calculations.

**Props**:
- `List<BudgetItem> Items` (required): Items to display
- `bool ShowActions` (optional, default: false): Show edit/delete actions

**Usage**:
```razor
<BudgetItemTable Items="@budget.Items" />
```

### ClientInfo.razor
Displays client information section.

**Props**:
- `Client Client` (required): Client to display

**Usage**:
```razor
<ClientInfo Client="@budget.Client" />
```

### BudgetList.razor
Displays list of saved budgets for selection.

**Props**:
- `EventCallback<Budget> OnBudgetSelected`: Callback when budget is selected

**Usage**:
```razor
<BudgetList OnBudgetSelected="@LoadBudget" />
```

## Key Services

### IBudgetService
Manages budget CRUD operations and calculations.

**Key Methods**:
- `CreateBudgetAsync(Client)`: Create new budget
- `GetBudgetAsync(Guid)`: Retrieve budget by ID
- `GetAllBudgetsAsync()`: Get all budgets as summaries
- `UpdateBudgetAsync(Budget)`: Update existing budget
- `CalculateTotal(Budget)`: Calculate budget total

### ILocalStorageService
Abstraction for LocalStorage operations.

**Key Methods**:
- `GetItemAsync<T>(string)`: Retrieve from LocalStorage
- `SetItemAsync<T>(string, T)`: Save to LocalStorage
- `IsAvailableAsync()`: Check if LocalStorage is available

### ICalculationService
Handles all budget calculations.

**Key Methods**:
- `CalculateSubtotal(decimal, decimal)`: Calculate item subtotal
- `CalculateTotal(IEnumerable<BudgetItem>)`: Calculate budget total
- `FormatCurrency(decimal)`: Format for display

## Data Models

### Budget
Complete budget entity with client, items, and metadata.

**Key Properties**:
- `Id`: Unique identifier
- `Client`: Client information
- `Items`: List of budget items
- `Total`: Calculated total (read-only)
- `CreatedDate`: Creation timestamp
- `LastModifiedDate`: Last modification timestamp

### BudgetItem
Line item in a budget.

**Key Properties**:
- `Id`: Unique identifier
- `Description`: Item description
- `Quantity`: Item quantity (> 0)
- `UnitPrice`: Price per unit (>= 0)
- `Subtotal`: Calculated subtotal (read-only)

### Client
Client/customer information.

**Key Properties**:
- `Name`: Client name (required)
- `Company`: Company name (optional)
- `Email`: Email address (required, validated)

## Setup Steps

1. **Register Services** (Program.cs):
```csharp
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<ICalculationService, CalculationService>();
builder.Services.AddBlazoredLocalStorage();
```

2. **Inject Services** (in components):
```csharp
@inject IBudgetService BudgetService
@inject ILocalStorageService LocalStorageService
```

3. **Use Components**:
```razor
<BudgetPreview Budget="@currentBudget" />
<BudgetList OnBudgetSelected="@HandleBudgetSelected" />
```

## Testing Approach

### Unit Tests
- Test services with mocked dependencies
- Test calculation logic with various inputs
- Test validation rules

### Component Tests (bUnit)
- Test component rendering
- Test user interactions
- Test data binding

### Integration Tests
- Test LocalStorage operations (with mocks)
- Test full budget save/load cycle
- Test error handling scenarios

## Common Patterns

### Auto-save Pattern
```csharp
private async Task OnBudgetChanged()
{
    await BudgetService.UpdateBudgetAsync(currentBudget);
    // Auto-save triggered
}
```

### Load Budget Pattern
```csharp
private async Task LoadBudget(Guid budgetId)
{
    var budget = await BudgetService.GetBudgetAsync(budgetId);
    if (budget != null)
    {
        currentBudget = budget;
        StateHasChanged();
    }
}
```

### Validation Pattern
```csharp
<EditForm Model="@budget.Client" OnValidSubmit="@SaveClient">
    <DataAnnotationsValidator />
    <ValidationSummary />
    <!-- Form fields -->
</EditForm>
```

## Error Handling

- **LocalStorage Full**: Show error message with option to clear old budgets
- **Invalid Data**: Show validation errors using ValidationMessage components
- **Load Failure**: Show user-friendly error, don't crash application
- **Corrupted Data**: Detect and handle gracefully, offer recovery options

## Performance Considerations

- Debounce auto-save (500ms after last change)
- Use @key directive for item lists
- Virtual scrolling for 100+ items (if needed)
- Lazy load budget list summaries

## Next Steps

1. Implement models with Data Annotations
2. Create service interfaces and implementations
3. Build Blazor components
4. Add LocalStorage integration
5. Implement preview formatting
6. Add tests following TDD approach


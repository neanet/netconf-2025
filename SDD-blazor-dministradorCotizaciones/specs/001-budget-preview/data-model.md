# Data Model: Vista Previa y Guardado/Carga de Presupuestos

**Feature**: 001-budget-preview  
**Date**: 2025-01-27

## Entities

### Presupuesto (Budget)

Represents a complete budget document that can be previewed, saved, and loaded from LocalStorage.

**Type**: C# Record  
**Namespace**: BudgetApp.Models

**Attributes**:
- `Id` (Guid, required): Unique identifier for the budget
- `Client` (Client, required): Client information associated with the budget
- `Items` (List<BudgetItem>, required): List of line items in the budget
- `CreatedDate` (DateTime, required): Date and time when the budget was created
- `LastModifiedDate` (DateTime, required): Date and time when the budget was last modified
- `Total` (decimal, calculated): Sum of all item subtotals (calculated property)

**Validation Rules**:
- Id must not be empty Guid
- Client must not be null
- Items must not be null (can be empty list)
- CreatedDate must be valid DateTime
- LastModifiedDate must be >= CreatedDate

**Relationships**:
- One-to-one with Client
- One-to-many with BudgetItem (a budget contains zero or more items)

**State Transitions**:
- New → Saved (when first saved to LocalStorage)
- Saved → Modified (when any item or client info changes)
- Modified → Saved (when changes are persisted)

---

### BudgetItem

Represents a line item in a budget with description, quantity, unit price, and calculated subtotal.

**Type**: C# Record  
**Namespace**: BudgetApp.Models

**Attributes**:
- `Id` (Guid, required): Unique identifier for the item within the budget
- `Description` (string, required): Description of the item
- `Quantity` (decimal, required): Quantity of items
- `UnitPrice` (decimal, required): Price per unit
- `Subtotal` (decimal, calculated): Calculated as Quantity × UnitPrice

**Validation Rules**:
- Id must not be empty Guid
- Description must not be null or whitespace (min length: 1, max length: 500)
- Quantity must be > 0
- UnitPrice must be >= 0
- Subtotal is calculated automatically (not user-editable)

**Relationships**:
- Many-to-one with Budget (each item belongs to exactly one budget)

**Calculations**:
- Subtotal = Quantity × UnitPrice
- Rounded to 2 decimal places for monetary precision

---

### Client

Represents client/customer information associated with a budget.

**Type**: C# Record  
**Namespace**: BudgetApp.Models

**Attributes**:
- `Name` (string, required): Client's full name
- `Company` (string, optional): Company name (can be null or empty)
- `Email` (string, required): Client's email address

**Validation Rules**:
- Name must not be null or whitespace (min length: 1, max length: 200)
- Company can be null or empty (max length: 200 if provided)
- Email must be valid email format (validated with EmailAddress attribute)
- Email must not be null or whitespace

**Relationships**:
- One-to-one with Budget (each budget has exactly one client)

---

### BudgetSummary

Represents a summary view of a budget for listing purposes (used in budget list view).

**Type**: C# Record  
**Namespace**: BudgetApp.Models

**Attributes**:
- `Id` (Guid, required): Budget identifier
- `ClientName` (string, required): Client's name for display
- `ItemCount` (int, required): Number of items in the budget
- `Total` (decimal, required): Total amount
- `CreatedDate` (DateTime, required): Creation date
- `LastModifiedDate` (DateTime, required): Last modification date

**Purpose**: Lightweight representation for budget list display without loading full budget data.

---

## Data Storage Schema (LocalStorage)

### Storage Keys

- `budgets`: JSON array of all saved budgets
- `budget_{id}`: Individual budget stored by ID (alternative approach)

### Storage Format

```json
{
  "budgets": [
    {
      "id": "guid",
      "client": {
        "name": "string",
        "company": "string|null",
        "email": "string"
      },
      "items": [
        {
          "id": "guid",
          "description": "string",
          "quantity": 0.0,
          "unitPrice": 0.0
        }
      ],
      "createdDate": "ISO8601",
      "lastModifiedDate": "ISO8601"
    }
  ]
}
```

### Versioning

- Current version: 1.0
- Future migrations: Use version field in storage schema
- Migration strategy: Check version on load, migrate if needed

---

## Validation Rules Summary

### Budget Validation
- Must have valid client information
- Items list can be empty (budget with 0 items is valid)
- Total must equal sum of item subtotals

### BudgetItem Validation
- Description: Required, 1-500 characters
- Quantity: Required, > 0
- UnitPrice: Required, >= 0
- Subtotal: Auto-calculated, read-only

### Client Validation
- Name: Required, 1-200 characters, not whitespace
- Company: Optional, max 200 characters if provided
- Email: Required, valid email format

---

## Data Flow

1. **Create Budget**: User creates new budget → Models created → Validated → Saved to LocalStorage
2. **Load Budget**: User selects budget → Loaded from LocalStorage → Deserialized → Validated → Displayed
3. **Update Budget**: User modifies budget → Models updated → Validated → Saved to LocalStorage
4. **Preview Budget**: Budget data → Calculation service → Formatted → Displayed in preview component

---

## Error Handling

- **Invalid Data**: If LocalStorage contains invalid JSON, show error and don't load
- **Missing Data**: If required fields are missing, show validation errors
- **Storage Full**: If LocalStorage quota exceeded, show error with recovery options
- **Corrupted Data**: If data structure doesn't match expected schema, show error and offer to reset


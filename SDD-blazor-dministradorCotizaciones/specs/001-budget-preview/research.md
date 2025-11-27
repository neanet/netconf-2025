# Research: Vista Previa Profesional y Guardado/Carga de Presupuestos

**Feature**: 001-budget-preview  
**Date**: 2025-01-27  
**Status**: Complete

## Research Tasks

### 1. Blazor WebAssembly Component Architecture

**Task**: Research best practices for modular and reusable Blazor components in .NET 10

**Findings**:
- Blazor components should follow single responsibility principle
- Use component parameters for data input and EventCallback for actions
- Create base components for common UI patterns (tables, forms, cards)
- Use partial classes to separate markup (.razor) from code (.razor.cs)
- Leverage CascadingValue and CascadingParameter for shared context

**Decision**: Implement components as separate .razor files with code-behind in .razor.cs files. Use component parameters for configuration and EventCallback for user interactions.

**Rationale**: This approach maintains separation of concerns, enables reusability, and aligns with Blazor best practices for .NET 10.

**Alternatives Considered**:
- Inline code in .razor files: Rejected - harder to maintain and test
- Full separation in separate classes: Rejected - adds unnecessary complexity for simple components

---

### 2. Service Layer for Business Logic

**Task**: Research service patterns for Blazor WebAssembly applications

**Findings**:
- Services should be registered in Program.cs using dependency injection
- Use interfaces for services to enable testing and mocking
- Services should be scoped per component tree (Scoped lifetime)
- Business logic should be separated from UI logic
- Services can use other services through constructor injection

**Decision**: Create service interfaces (IBudgetService, ILocalStorageService, ICalculationService) and implementations. Register services as Scoped in Program.cs.

**Rationale**: This pattern enables testability, maintainability, and follows SOLID principles. Scoped lifetime is appropriate for Blazor WebAssembly applications.

**Alternatives Considered**:
- Singleton services: Rejected - could cause state issues in multi-user scenarios
- Transient services: Rejected - unnecessary overhead for stateless services

---

### 3. Typed Models in C# 13

**Task**: Research C# 13 features for typed models and data structures

**Findings**:
- C# 13 supports records for immutable data structures
- Data Annotations available in System.ComponentModel.Annotations
- Primary constructors for concise model definitions
- Pattern matching enhancements
- Required members for non-nullable properties

**Decision**: Use C# records with Data Annotations for models. Use required properties for mandatory fields. Implement validation attributes (Required, EmailAddress, Range, etc.).

**Rationale**: Records provide immutability benefits and concise syntax. Data Annotations enable declarative validation that integrates with Blazor forms.

**Alternatives Considered**:
- Classes with properties: Accepted as alternative, but records preferred for value objects
- Fluent validation: Rejected - Data Annotations are sufficient and built-in

---

### 4. Form Validation with Data Annotations

**Task**: Research Data Annotations validation in Blazor forms

**Findings**:
- Blazor supports EditForm component with DataAnnotationsValidator
- Validation messages displayed using ValidationMessage and ValidationSummary
- Custom validation attributes can be created
- Validation occurs on submit and can be triggered manually
- EditContext provides validation state

**Decision**: Use EditForm with DataAnnotationsValidator. Create models with Data Annotations attributes. Display validation messages using ValidationMessage components.

**Rationale**: Data Annotations provide declarative validation that is easy to maintain and test. Blazor's built-in support makes implementation straightforward.

**Alternatives Considered**:
- FluentValidation: Rejected - adds external dependency, Data Annotations sufficient
- Manual validation: Rejected - more error-prone and harder to maintain

---

### 5. LocalStorage Access in Blazor WebAssembly

**Task**: Research LocalStorage libraries for Blazor WebAssembly

**Findings**:
- Blazored.LocalStorage is the most popular and maintained library
- Provides async API for LocalStorage operations
- Supports JSON serialization/deserialization
- Handles quota exceeded errors gracefully
- Works with dependency injection

**Decision**: Use Blazored.LocalStorage for LocalStorage access. Create abstraction service (ILocalStorageService) to enable testing.

**Rationale**: Blazored.LocalStorage is well-maintained, has good documentation, and integrates seamlessly with Blazor dependency injection. Abstraction enables testing without actual LocalStorage.

**Alternatives Considered**:
- Direct JSInterop: Rejected - more boilerplate, error-prone
- Other libraries: Rejected - Blazored.LocalStorage is the de facto standard

---

### 6. Professional Preview Formatting

**Task**: Research best practices for professional document preview formatting

**Findings**:
- Use CSS Grid or Flexbox for layout
- Typography: Clear hierarchy with headings, readable fonts
- Tables: Clear borders, alternating row colors, proper alignment
- Monetary values: Right-aligned, consistent formatting
- Responsive design: Works on different screen sizes
- Print-friendly styles available

**Decision**: Implement preview using Blazor components with CSS classes. Use CSS Grid for layout, professional typography, and styled tables. Ensure responsive design.

**Rationale**: CSS-based approach is flexible, maintainable, and doesn't require external libraries. Can be extended for PDF generation later.

**Alternatives Considered**:
- HTML to PDF libraries: Deferred - not needed for preview, can be added later
- External styling libraries: Rejected - adds dependency, CSS sufficient

---

### 7. Performance Optimization for Large Budgets

**Task**: Research performance optimization for rendering large lists in Blazor

**Findings**:
- Virtual scrolling for very large lists (100+ items)
- Use @key directive for efficient re-rendering
- Minimize component re-renders with ShouldRender override
- Lazy loading for off-screen content
- Debounce auto-save operations

**Decision**: Implement virtual scrolling if needed for 100+ items. Use @key for item lists. Debounce auto-save to 500ms after last change.

**Rationale**: For up to 100 items, standard rendering should be sufficient. Virtual scrolling can be added if performance issues arise. Debouncing prevents excessive LocalStorage writes.

**Alternatives Considered**:
- Pagination: Rejected - preview should show all items at once
- Infinite scroll: Rejected - not applicable for preview context

---

## Summary of Decisions

1. **Component Architecture**: Modular Blazor components with code-behind files
2. **Service Layer**: Interface-based services with Scoped lifetime
3. **Models**: C# 13 records with Data Annotations
4. **Validation**: Data Annotations with Blazor EditForm
5. **LocalStorage**: Blazored.LocalStorage with abstraction service
6. **Preview Formatting**: CSS-based professional styling
7. **Performance**: Standard rendering with debounced auto-save

All research tasks completed. No NEEDS CLARIFICATION items remain.


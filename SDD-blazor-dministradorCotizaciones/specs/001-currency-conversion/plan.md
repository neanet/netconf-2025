# Implementation Plan: Conversión de Moneda y Cálculo de Presupuesto

**Branch**: `001-currency-conversion` | **Date**: 2025-01-27 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/001-currency-conversion/spec.md`

## Summary

Esta feature permite a los usuarios seleccionar la moneda (Pesos o Dólares) para sus presupuestos y configurar una tasa de cambio para convertir automáticamente todos los valores monetarios. El sistema recalcula precios unitarios, subtotales y totales cuando se cambia la moneda, y persiste la moneda seleccionada con cada presupuesto. La implementación extiende el sistema existente de cálculos y persistencia sin requerir cambios arquitectónicos mayores.

## Technical Context

**Language/Version**: C# 13 (.NET 10)  
**Primary Dependencies**: Blazor WebAssembly, Blazored.LocalStorage, System.Text.Json  
**Storage**: Local Storage del navegador (mediante ILocalStorageService)  
**Testing**: bUnit (component tests), integration tests  
**Target Platform**: Blazor WebAssembly (navegadores modernos con soporte WASM)  
**Project Type**: Web application (Blazor WASM standalone)  
**Performance Goals**: Conversión de moneda y recalculación de valores en menos de 1 segundo  
**Constraints**: Offline-capable, sin backend, solo tecnologías .NET 10  
**Scale/Scope**: Presupuestos individuales con conversión bidireccional Pesos ↔ Dólares

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### I. Arquitectura Blazor WebAssembly Standalone
✅ **PASS**: La feature extiende la aplicación Blazor WASM existente sin requerir backend. Toda la lógica de conversión se ejecuta en el cliente.

### II. Componentes Reutilizables y Modularidad
✅ **PASS**: Se crearán componentes Blazor reutilizables para selección de moneda y configuración de tasa de cambio. La lógica de conversión se encapsulará en servicios inyectables.

### III. Test-First Development
✅ **PASS**: Se seguirá TDD estrictamente. Tests de componentes para UI de selección de moneda, tests de integración para persistencia de moneda en LocalStorage.

### IV. Persistencia Local Storage
✅ **PASS**: La moneda seleccionada se persistirá con cada presupuesto en LocalStorage mediante el servicio existente. No requiere cambios en la infraestructura de persistencia.

### V. Generación de Documentos PDF en Cliente
⚠️ **N/A**: Esta feature no afecta la generación de PDFs directamente, pero los valores convertidos se mostrarán en los documentos generados.

### VI. Validación y Seguridad de Datos
✅ **PASS**: Se validará que la tasa de cambio sea mayor que cero. Se usarán Data Annotations para validación de entrada.

### VII. Versionado y Gestión de Cambios
✅ **PASS**: La moneda se almacena con cada presupuesto, permitiendo trazabilidad. Los cambios de moneda actualizan LastModifiedDate.

**Gate Status**: ✅ **PASS** - No hay violaciones de constitución. La feature se integra naturalmente con la arquitectura existente.

## Project Structure

### Documentation (this feature)

```text
specs/001-currency-conversion/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
src/
├── Models/
│   ├── Budget.cs              # Extend: Add Currency property
│   ├── BudgetItem.cs          # No changes needed
│   ├── Client.cs              # No changes needed
│   └── Currency.cs            # NEW: Currency enum or record
├── Services/
│   ├── ICalculationService.cs    # Extend: Add currency formatting methods
│   ├── CalculationService.cs      # Extend: Implement currency conversion logic
│   ├── ICurrencyService.cs       # NEW: Service for currency operations
│   ├── CurrencyService.cs        # NEW: Implementation of currency conversion
│   ├── IBudgetService.cs         # No changes (currency persisted with budget)
│   └── BudgetService.cs          # No changes (currency persisted with budget)
└── Components/
    ├── CurrencySelector.razor     # NEW: Component for currency selection
    ├── ExchangeRateConfig.razor   # NEW: Component for exchange rate configuration
    └── [existing components]      # Extend: Use currency-aware formatting

tests/
├── components/
│   ├── CurrencySelectorTests.cs   # NEW: Component tests
│   └── ExchangeRateConfigTests.cs # NEW: Component tests
└── integration/
    └── CurrencyPersistenceTests.cs # NEW: Integration tests for currency persistence
```

**Structure Decision**: Se extiende la estructura existente de modelos, servicios y componentes. Se agregan nuevos servicios y componentes para manejar la conversión de moneda, manteniendo la separación de responsabilidades y modularidad.

## Complexity Tracking

> **No violations detected** - La feature se integra naturalmente con la arquitectura existente sin requerir cambios arquitectónicos mayores.

## Phase 0: Research & Design Decisions

[See research.md for detailed research findings]

### Key Research Questions

1. **Currency Storage Format**: ¿Cómo almacenar la moneda en el modelo Budget?
2. **Exchange Rate Storage**: ¿Dónde y cómo persistir la tasa de cambio?
3. **Base Currency Strategy**: ¿Cómo mantener valores base para reconversión precisa?
4. **Currency Formatting**: ¿Cómo formatear valores según moneda seleccionada?
5. **Conversion Precision**: ¿Cómo manejar precisión en conversiones bidireccionales?

### Design Decisions

- **Currency Storage**: Enum `Currency` (Pesos, Dollars) almacenado en `Budget`
- **Exchange Rate**: Almacenado en sesión (servicio) y opcionalmente por presupuesto
- **Base Currency**: Valores originales en moneda base (Pesos) para reconversión precisa
- **Formatting**: Extender `ICalculationService` con métodos de formateo por moneda
- **Precision**: Usar `decimal` con redondeo a 2 decimales para todas las conversiones

## Phase 1: Data Model & Contracts

[See data-model.md and contracts/ for detailed specifications]

### Data Model Changes

- **Budget**: Agregar propiedad `Currency` (enum, default: Pesos)
- **Currency**: Nuevo enum con valores `Pesos`, `Dollars`
- **ExchangeRate**: Configuración de tasa almacenada en servicio (no en modelo)

### Service Contracts

- **ICurrencyService**: Interfaz para operaciones de conversión de moneda
- **ICalculationService**: Extender con métodos de formateo por moneda

## Phase 2: Implementation Tasks

[See tasks.md - created by /speckit.tasks command]

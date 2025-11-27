# Implementation Plan: Vista Previa Profesional y Guardado/Carga de Presupuestos

**Branch**: `001-budget-preview` | **Date**: 2025-01-27 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/001-budget-preview/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/commands/plan.md` for the execution workflow.

## Summary

Implementar una vista previa profesional del presupuesto que muestre todos los items, cálculos intermedios y el total final, junto con funcionalidad completa de guardado y carga de presupuestos desde LocalStorage. La arquitectura utilizará componentes Blazor modulares y reutilizables, servicios para la lógica de negocio, modelos tipados en C# 13, y validación de formularios con Data Annotations.

## Technical Context

**Language/Version**: C# 13 / .NET 10  
**Primary Dependencies**: 
- Microsoft.AspNetCore.Components.WebAssembly
- Microsoft.AspNetCore.Components.WebAssembly.DevServer
- Blazored.LocalStorage (para acceso a LocalStorage)
- System.ComponentModel.Annotations (para Data Annotations)
- System.Text.Json (para serialización)

**Storage**: LocalStorage del navegador (mediante Blazored.LocalStorage)  
**Testing**: xUnit, bUnit (para componentes Blazor), FluentAssertions  
**Target Platform**: Blazor WebAssembly (WASM) - Navegadores modernos  
**Project Type**: Web application (Blazor WebAssembly standalone)  
**Performance Goals**: 
- Vista previa renderizada en < 2 segundos para presupuestos con hasta 100 items
- Guardado automático en < 1 segundo después de modificaciones
- Carga de presupuestos guardados en < 3 segundos

**Constraints**: 
- Aplicación standalone sin backend
- Solo tecnologías .NET 10
- LocalStorage limitado a ~5-10MB por dominio
- Funcionamiento offline completo
- Compatibilidad con navegadores modernos (Chrome, Firefox, Edge, Safari)

**Scale/Scope**: 
- Presupuestos con hasta 100 items sin degradación de rendimiento
- Múltiples presupuestos guardados (hasta límite de LocalStorage)
- Un usuario por sesión de navegador

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### Pre-Research Gates (Phase 0)

✅ **I. Arquitectura Blazor WebAssembly Standalone**: 
- La implementación será una aplicación Blazor WebAssembly standalone
- Componentes Blazor modulares y reutilizables
- Persistencia mediante LocalStorage (Blazored.LocalStorage)
- **Status**: COMPLIANT

✅ **II. Componentes Reutilizables y Modularidad**:
- Componentes Blazor independientes y reutilizables
- Lógica de negocio en servicios inyectables
- Modelos tipados en C# 13 centralizados
- **Status**: COMPLIANT

✅ **III. Test-First Development**:
- TDD será aplicado estrictamente
- Tests unitarios para servicios
- Tests de componentes con bUnit
- Tests de integración para LocalStorage
- **Status**: COMPLIANT

✅ **IV. Persistencia Local Storage**:
- Acceso a LocalStorage abstraído mediante servicio inyectable
- Manejo de casos donde LocalStorage no esté disponible
- Validación antes de persistir
- **Status**: COMPLIANT

✅ **VI. Validación y Seguridad de Datos**:
- Validación con Data Annotations de .NET
- Validación en cliente antes de persistir
- Retroalimentación inmediata al usuario
- **Status**: COMPLIANT

✅ **Technology Stack**:
- Solo .NET 10 y tecnologías compatibles
- Sin backend requerido
- **Status**: COMPLIANT

**Overall Status**: ✅ ALL GATES PASS - Proceed to Phase 0

### Post-Design Gates (Phase 1)

✅ **I. Arquitectura Blazor WebAssembly Standalone**: 
- Design uses Blazor WebAssembly standalone architecture
- Components are modular and reusable
- LocalStorage abstraction via ILocalStorageService
- **Status**: COMPLIANT

✅ **II. Componentes Reutilizables y Modularidad**:
- Components designed as independent, reusable Blazor components
- Business logic separated into services (IBudgetService, ICalculationService)
- Models are typed C# records in shared Models namespace
- **Status**: COMPLIANT

✅ **III. Test-First Development**:
- Test structure defined (unit, integration, components)
- Services designed with interfaces for testability
- Components can be tested with bUnit
- **Status**: COMPLIANT

✅ **IV. Persistencia Local Storage**:
- ILocalStorageService abstraction created
- Error handling for LocalStorage unavailable scenarios
- Data validation before persistence
- **Status**: COMPLIANT

✅ **VI. Validación y Seguridad de Datos**:
- Models use Data Annotations for validation
- Validation rules defined in data model
- Client-side validation before persistence
- **Status**: COMPLIANT

✅ **Technology Stack**:
- All technologies are .NET 10 compatible
- No backend dependencies
- Blazored.LocalStorage for LocalStorage access
- **Status**: COMPLIANT

**Post-Design Status**: ✅ ALL GATES PASS - Design aligns with constitution

## Project Structure

### Documentation (this feature)

```text
specs/001-budget-preview/
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
│   ├── Budget.cs
│   ├── BudgetItem.cs
│   ├── Client.cs
│   └── BudgetSummary.cs
├── Services/
│   ├── IBudgetService.cs
│   ├── BudgetService.cs
│   ├── ILocalStorageService.cs
│   ├── LocalStorageService.cs
│   └── ICalculationService.cs
│   └── CalculationService.cs
├── Components/
│   ├── BudgetPreview.razor
│   ├── BudgetItemTable.razor
│   ├── ClientInfo.razor
│   ├── BudgetSummary.razor
│   ├── BudgetList.razor
│   └── SaveBudgetButton.razor
└── Pages/
    ├── BudgetEditor.razor
    └── BudgetList.razor

tests/
├── unit/
│   ├── Services/
│   │   ├── BudgetServiceTests.cs
│   │   ├── CalculationServiceTests.cs
│   │   └── LocalStorageServiceTests.cs
│   └── Models/
│       └── BudgetTests.cs
├── integration/
│   ├── LocalStorageIntegrationTests.cs
│   └── BudgetPersistenceTests.cs
└── components/
    ├── BudgetPreviewTests.cs
    ├── BudgetItemTableTests.cs
    └── ClientInfoTests.cs
```

**Structure Decision**: Se utiliza una estructura de proyecto único (single project) ya que es una aplicación Blazor WebAssembly standalone sin backend. Los modelos están en `Models/`, los servicios en `Services/`, y los componentes Blazor en `Components/` y `Pages/`. Los tests están organizados por tipo (unit, integration, components) para facilitar el TDD.

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

No violations detected. All architecture decisions align with the constitution.

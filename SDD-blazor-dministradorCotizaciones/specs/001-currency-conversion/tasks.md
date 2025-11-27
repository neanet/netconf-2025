# Tasks: Conversión de Moneda y Cálculo de Presupuesto

**Input**: Design documents from `/specs/001-currency-conversion/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/

**Tests**: Component tests and integration tests are included per Constitution requirement (TDD).

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- **Project structure**: `src/Models/`, `src/Services/`, `src/Components/`, `tests/components/`, `tests/integration/`

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project structure verification and preparation

- [X] T001 Verify existing project structure matches plan.md requirements
- [X] T002 [P] Review existing BudgetService and CalculationService implementations for integration points

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core models and services that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [X] T003 [P] Create Currency enum in src/Models/Currency.cs
- [X] T004 [P] Extend Budget model with Currency property in src/Models/Budget.cs
- [X] T005 [P] Create ICurrencyService interface in src/Services/ICurrencyService.cs
- [X] T006 [P] Extend ICalculationService interface with FormatCurrency overload in src/Services/ICalculationService.cs

**Checkpoint**: Foundation ready - user story implementation can now begin

---

## Phase 3: User Story 2 - Configurar Tasa de Cambio (Priority: P1) 🎯 MVP

**Goal**: Usuario puede configurar la tasa de cambio entre Pesos y Dólares para realizar conversiones correctamente.

**Independent Test**: Configurar una tasa de cambio (ej: 1 USD = 50 pesos), crear un presupuesto con valores en una moneda, cambiar a la otra moneda, y verificar que los valores convertidos son correctos según la tasa configurada.

### Tests for User Story 2

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [ ] T007 [P] [US2] Component test for ExchangeRateConfig validation in tests/components/ExchangeRateConfigTests.cs
- [ ] T008 [P] [US2] Integration test for exchange rate configuration and persistence in tests/integration/CurrencyServiceTests.cs

### Implementation for User Story 2

- [X] T009 [US2] Implement CurrencyService in src/Services/CurrencyService.cs (depends on T005)
- [X] T010 [P] [US2] Create ExchangeRateConfig component in src/Components/ExchangeRateConfig.razor
- [X] T011 [US2] Register ICurrencyService in Program.cs dependency injection
- [X] T012 [US2] Add validation and error handling for exchange rate configuration

**Checkpoint**: At this point, User Story 2 should be fully functional and testable independently. Exchange rate can be configured and validated.

---

## Phase 4: User Story 1 - Seleccionar Moneda para el Presupuesto (Priority: P1) 🎯 MVP

**Goal**: Usuario puede seleccionar la moneda (pesos o dólares) para un presupuesto, cambiar la moneda en cualquier momento, y el sistema recalcula automáticamente todos los valores según la tasa de cambio configurada. La moneda seleccionada se persiste con el presupuesto.

**Independent Test**: Crear un presupuesto, seleccionar una moneda (pesos o dólares), agregar items con precios, cambiar la moneda, y verificar que todos los valores se recalculan correctamente según la tasa de cambio. Cargar un presupuesto guardado y verificar que mantiene su moneda.

### Tests for User Story 1

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [ ] T013 [P] [US1] Component test for CurrencySelector in tests/components/CurrencySelectorTests.cs
- [ ] T014 [P] [US1] Integration test for currency selection and budget persistence in tests/integration/CurrencyPersistenceTests.cs
- [ ] T015 [P] [US1] Integration test for currency conversion calculations in tests/integration/CurrencyConversionTests.cs

### Implementation for User Story 1

- [X] T016 [P] [US1] Create CurrencySelector component in src/Components/CurrencySelector.razor
- [X] T017 [US1] Extend CalculationService with FormatCurrency overload in src/Services/CalculationService.cs (depends on T006)
- [X] T018 [US1] Implement currency conversion logic in BudgetEditor page in src/Pages/BudgetEditor.razor
- [X] T019 [US1] Update BudgetService to persist and load Currency property (automatic via JSON serialization, verify backward compatibility)
- [X] T020 [US1] Add currency change handler that recalculates all values using ICurrencyService
- [X] T021 [US1] Add validation to require exchange rate before currency conversion

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently. Users can select currency, see values recalculated, and currency persists with budget.

---

## Phase 5: User Story 3 - Visualizar Valores en Moneda Seleccionada (Priority: P2)

**Goal**: Usuario ve todos los valores monetarios del presupuesto (precios unitarios, subtotales, totales) formateados correctamente según la moneda seleccionada, incluyendo el símbolo o código de moneda apropiado.

**Independent Test**: Seleccionar diferentes monedas y verificar que todos los valores se muestran con el formato y símbolo correcto de la moneda seleccionada.

### Tests for User Story 3

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [ ] T022 [P] [US3] Component test for currency formatting in BudgetItemTable in tests/components/BudgetItemTableTests.cs
- [ ] T023 [P] [US3] Component test for currency formatting in BudgetSummary in tests/components/BudgetSummaryTests.cs
- [ ] T024 [P] [US3] Component test for currency formatting in BudgetPreview in tests/components/BudgetPreviewTests.cs

### Implementation for User Story 3

- [X] T025 [US3] Update BudgetItemTable component to use FormatCurrency with currency in src/Components/BudgetItemTable.razor
- [X] T026 [US3] Update BudgetSummary component to use FormatCurrency with currency in src/Components/BudgetSummary.razor
- [X] T027 [US3] Update BudgetPreview component to display currency code in header in src/Components/BudgetPreview.razor
- [ ] T028 [US3] Update ClientInfo component to show currency context if needed in src/Components/ClientInfo.razor
- [X] T029 [US3] Ensure all monetary values update immediately when currency changes

**Checkpoint**: At this point, User Story 3 should be fully functional. All monetary values display with correct currency formatting throughout the application.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [X] T030 [P] Update BudgetList component to show currency in summary view in src/Components/BudgetList.razor
- [X] T031 [P] Add currency indicator in BudgetEditor page header in src/Pages/BudgetEditor.razor
- [X] T032 [P] Verify backward compatibility: existing budgets without Currency property default to Pesos
- [X] T033 [P] Add error messages for missing exchange rate when attempting currency conversion
- [X] T034 [P] Add visual feedback when currency conversion is in progress
- [X] T035 [P] Code cleanup and refactoring across all currency-related components
- [X] T036 [P] Run quickstart.md validation to ensure implementation matches guide
- [X] T037 [P] Performance validation: verify currency conversion completes in under 1 second (SC-001)

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
  - User Story 2 (Phase 3) should complete before User Story 1 (Phase 4) for full functionality
  - User Story 1 (Phase 4) can start after Foundational but needs US2 for complete testing
  - User Story 3 (Phase 5) depends on User Story 1 completion
- **Polish (Phase 6)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 2 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories. **CRITICAL**: Must complete before US1 for full functionality.
- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - Requires US2 for complete functionality (exchange rate configuration). Can be partially tested without US2.
- **User Story 3 (P2)**: Depends on User Story 1 completion - Needs currency selection to work before formatting can be tested.

### Within Each User Story

- Tests (if included) MUST be written and FAIL before implementation
- Models before services
- Services before components
- Core implementation before integration
- Story complete before moving to next priority

### Parallel Opportunities

- **Phase 2 (Foundational)**: T003, T004, T005, T006 can all run in parallel (different files)
- **Phase 3 (US2)**: T007, T008 can run in parallel; T010 can run after T009
- **Phase 4 (US1)**: T013, T014, T015 can run in parallel; T016 can run in parallel with T017
- **Phase 5 (US3)**: T022, T023, T024 can run in parallel; T025, T026, T027, T028 can run in parallel
- **Phase 6 (Polish)**: All tasks marked [P] can run in parallel
- Different user stories can be worked on in parallel by different team members after foundational phase

---

## Parallel Example: User Story 1

```bash
# Launch all tests for User Story 1 together:
Task: "Component test for CurrencySelector in tests/components/CurrencySelectorTests.cs"
Task: "Integration test for currency selection and budget persistence in tests/integration/CurrencyPersistenceTests.cs"
Task: "Integration test for currency conversion calculations in tests/integration/CurrencyConversionTests.cs"

# Launch models and components in parallel:
Task: "Create CurrencySelector component in src/Components/CurrencySelector.razor"
Task: "Extend CalculationService with FormatCurrency overload in src/Services/CalculationService.cs"
```

---

## Parallel Example: User Story 2

```bash
# Launch all tests for User Story 2 together:
Task: "Component test for ExchangeRateConfig validation in tests/components/ExchangeRateConfigTests.cs"
Task: "Integration test for exchange rate configuration and persistence in tests/integration/CurrencyServiceTests.cs"

# Launch component in parallel with service implementation:
Task: "Create ExchangeRateConfig component in src/Components/ExchangeRateConfig.razor"
```

---

## Implementation Strategy

### MVP First (User Stories 1 & 2 - Both P1)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational (CRITICAL - blocks all stories)
3. Complete Phase 3: User Story 2 (Configurar Tasa de Cambio)
4. Complete Phase 4: User Story 1 (Seleccionar Moneda)
5. **STOP and VALIDATE**: Test both stories independently and together
6. Deploy/demo if ready

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 2 → Test independently → Exchange rate configuration works
3. Add User Story 1 → Test independently → Currency selection and conversion works
4. Add User Story 3 → Test independently → Currency formatting works
5. Each story adds value without breaking previous stories

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Once Foundational is done:
   - Developer A: User Story 2 (Exchange Rate Configuration)
   - Developer B: Prepare User Story 1 tests (can start in parallel)
3. Once User Story 2 is complete:
   - Developer A: User Story 1 (Currency Selection)
   - Developer B: User Story 3 (Currency Formatting) - can start after US1 models/services
4. Stories complete and integrate independently

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Verify tests fail before implementing
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- **Important**: User Story 2 (Exchange Rate) should complete before User Story 1 for full functionality, but both are P1
- Backward compatibility: Existing budgets without Currency property will default to Pesos (handled by model default value)
- Currency conversion uses base currency (Pesos) stored in BudgetItem.UnitPrice, conversion calculated on-the-fly for display

---

## Task Summary

- **Total Tasks**: 37
- **Phase 1 (Setup)**: 2 tasks
- **Phase 2 (Foundational)**: 4 tasks
- **Phase 3 (US2 - Exchange Rate)**: 6 tasks (2 tests + 4 implementation)
- **Phase 4 (US1 - Currency Selection)**: 9 tasks (3 tests + 6 implementation)
- **Phase 5 (US3 - Currency Formatting)**: 8 tasks (3 tests + 5 implementation)
- **Phase 6 (Polish)**: 8 tasks

**MVP Scope**: Phases 1-4 (User Stories 1 & 2) - 21 tasks total


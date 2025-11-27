# Tasks: Vista Previa Profesional y Guardado/Carga de Presupuestos

**Input**: Design documents from `/specs/001-budget-preview/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), data-model.md, contracts/, quickstart.md

**Tests**: Tests are OPTIONAL - only component tests and integration tests as per constitution (no unit tests).

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3, US4)
- Include exact file paths in descriptions

## Path Conventions

- **Single project**: `src/`, `tests/` at repository root
- Paths based on plan.md structure: `src/Models/`, `src/Services/`, `src/Components/`, `src/Pages/`

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic structure

- [x] T001 Create project structure per implementation plan (src/Models/, src/Services/, src/Components/, src/Pages/, tests/components/, tests/integration/)
- [x] T002 Initialize Blazor WebAssembly project with .NET 10 and required dependencies (Microsoft.AspNetCore.Components.WebAssembly, Blazored.LocalStorage, System.ComponentModel.Annotations)
- [x] T003 [P] Configure dependency injection in Program.cs (register services as Scoped)
- [x] T004 [P] Setup CSS structure for professional preview styling in wwwroot/css/

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [x] T005 [P] Create Client model in src/Models/Client.cs with Data Annotations (Name, Company, Email)
- [x] T006 [P] Create BudgetItem model in src/Models/BudgetItem.cs with Data Annotations (Description, Quantity, UnitPrice, calculated Subtotal)
- [x] T007 [P] Create Budget model in src/Models/Budget.cs with Data Annotations (Id, Client, Items, CreatedDate, LastModifiedDate, calculated Total)
- [x] T008 [P] Create BudgetSummary model in src/Models/BudgetSummary.cs for list display
- [x] T009 Create ILocalStorageService interface in src/Services/ILocalStorageService.cs
- [x] T010 Create LocalStorageService implementation in src/Services/LocalStorageService.cs (wraps Blazored.LocalStorage)
- [x] T011 Create ICalculationService interface in src/Services/ICalculationService.cs
- [x] T012 Create CalculationService implementation in src/Services/CalculationService.cs (CalculateSubtotal, CalculateTotal, FormatCurrency)
- [x] T013 Create IBudgetService interface in src/Services/IBudgetService.cs
- [x] T014 Create BudgetService implementation in src/Services/BudgetService.cs (depends on T010, T012)
- [x] T015 Register all services in Program.cs (ILocalStorageService, ICalculationService, IBudgetService)

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - Visualizar Vista Previa Profesional del Presupuesto (Priority: P1) 🎯 MVP

**Goal**: Display a professional preview of the budget showing client information, all items in a table format, and the total final

**Independent Test**: Create a budget with at least 2 items and client information, then verify the preview displays all elements clearly: client info, table with items (description, quantity, unit price, subtotal), and highlighted final total.

### Tests for User Story 1 (Component Tests)

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [ ] T016 [P] [US1] Component test for BudgetPreview in tests/components/BudgetPreviewTests.cs
- [ ] T017 [P] [US1] Component test for BudgetItemTable in tests/components/BudgetItemTableTests.cs
- [ ] T018 [P] [US1] Component test for ClientInfo in tests/components/ClientInfoTests.cs

### Implementation for User Story 1

- [x] T019 [P] [US1] Create ClientInfo component in src/Components/ClientInfo.razor (displays client name, company, email)
- [x] T020 [P] [US1] Create BudgetItemTable component in src/Components/BudgetItemTable.razor (displays items in table with columns: description, quantity, unit price, subtotal)
- [x] T021 [US1] Create BudgetPreview component in src/Components/BudgetPreview.razor (combines ClientInfo and BudgetItemTable, displays total) (depends on T019, T020)
- [x] T022 [US1] Add professional CSS styling for preview in wwwroot/css/preview.css (professional layout, typography, table styling)
- [x] T023 [US1] Create BudgetEditor page in src/Pages/BudgetEditor.razor that uses BudgetPreview component
- [x] T024 [US1] Add routing for BudgetEditor page in App.razor or routing configuration

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently - users can view a professional preview of any budget

---

## Phase 4: User Story 2 - Mostrar Cálculos Intermedios y Total Final (Priority: P1)

**Goal**: Display intermediate calculations (subtotals per item) and final total with proper monetary formatting (2 decimal places, thousand separators)

**Independent Test**: Create a budget with items having different quantities and prices, verify each subtotal displays correctly (e.g., quantity 5 × price 100 = subtotal 500), and the final total is the correct sum of all subtotals with proper formatting.

### Tests for User Story 2 (Component Tests)

- [x] T025 [P] [US2] Component test for CalculationService.FormatCurrency in tests/components/CalculationServiceTests.cs
- [x] T026 [P] [US2] Component test for BudgetPreview calculations display in tests/components/BudgetPreviewTests.cs (verify subtotals and total formatting)

### Implementation for User Story 2

- [x] T027 [US2] Update CalculationService.FormatCurrency in src/Services/CalculationService.cs to format with 2 decimal places and thousand separators
- [x] T028 [US2] Update BudgetItemTable component in src/Components/BudgetItemTable.razor to display formatted subtotals using CalculationService
- [x] T029 [US2] Update BudgetPreview component in src/Components/BudgetPreview.razor to display formatted final total using CalculationService (depends on T027, T028)
- [x] T030 [US2] Add BudgetSummary component in src/Components/BudgetSummary.razor to display total prominently
- [x] T031 [US2] Integrate BudgetSummary into BudgetPreview component in src/Components/BudgetPreview.razor

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently - preview shows all calculations correctly formatted

---

## Phase 5: User Story 3 - Guardar Presupuesto en LocalStorage (Priority: P2)

**Goal**: Save budgets to LocalStorage automatically when created or modified, and allow manual saving with visual confirmation

**Independent Test**: Create or edit a budget, verify it saves automatically to LocalStorage, then reload the page to confirm the budget is available.

### Tests for User Story 3 (Integration Tests)

- [x] T032 [P] [US3] Integration test for budget save to LocalStorage in tests/integration/BudgetPersistenceTests.cs
- [x] T033 [P] [US3] Integration test for auto-save functionality in tests/integration/BudgetPersistenceTests.cs

### Implementation for User Story 3

- [x] T034 [US3] Update BudgetService.SaveBudgetAsync in src/Services/BudgetService.cs to save to LocalStorage using ILocalStorageService
- [x] T035 [US3] Implement auto-save logic in BudgetService (debounce 500ms after modifications) in src/Services/BudgetService.cs
- [x] T036 [US3] Create SaveBudgetButton component in src/Components/SaveBudgetButton.razor for manual save with visual confirmation
- [x] T037 [US3] Add save confirmation UI (toast/notification) when budget is saved successfully
- [x] T038 [US3] Add error handling for LocalStorage full/unavailable scenarios in src/Services/BudgetService.cs
- [x] T039 [US3] Integrate auto-save and manual save into BudgetEditor page in src/Pages/BudgetEditor.razor

**Checkpoint**: At this point, User Stories 1, 2, AND 3 should work independently - budgets can be saved and persist after reload

---

## Phase 6: User Story 4 - Cargar Presupuestos desde LocalStorage (Priority: P2)

**Goal**: Load previously saved budgets from LocalStorage, display list of budgets, and allow selection to load complete budget data

**Independent Test**: Save at least 2 different budgets, then open the application and verify both budgets appear in a list, and selecting one loads it correctly with all information (client, items, totals).

### Tests for User Story 4 (Integration Tests)

- [x] T040 [P] [US4] Integration test for loading budgets from LocalStorage in tests/integration/BudgetPersistenceTests.cs
- [x] T041 [P] [US4] Integration test for budget list display in tests/integration/BudgetPersistenceTests.cs

### Implementation for User Story 4

- [x] T042 [US4] Update BudgetService.GetAllBudgetsAsync in src/Services/BudgetService.cs to load all budgets from LocalStorage and return BudgetSummary list
- [x] T043 [US4] Update BudgetService.GetBudgetAsync in src/Services/BudgetService.cs to load specific budget by ID from LocalStorage
- [x] T044 [US4] Create BudgetList component in src/Components/BudgetList.razor to display list of saved budgets with client name, date, item count
- [x] T045 [US4] Create BudgetList page in src/Pages/BudgetList.razor that uses BudgetList component and handles budget selection
- [x] T046 [US4] Add routing for BudgetList page in App.razor or routing configuration
- [x] T047 [US4] Add navigation between BudgetEditor and BudgetList pages
- [x] T048 [US4] Add error handling for corrupted/invalid data in LocalStorage in src/Services/BudgetService.cs
- [x] T049 [US4] Load budgets from LocalStorage on application startup in App.razor or main layout

**Checkpoint**: At this point, all user stories should work independently - users can create, preview, save, and load budgets

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [ ] T050 [P] Add scroll/pagination support for BudgetItemTable when there are many items (100+) in src/Components/BudgetItemTable.razor
- [ ] T051 [P] Add loading indicators for LocalStorage operations in src/Components/SaveBudgetButton.razor and src/Components/BudgetList.razor
- [ ] T052 [P] Improve error messages for LocalStorage full scenarios with recovery options (delete old budgets, export)
- [ ] T053 [P] Add differentiation for multiple budgets with same client name (show date, item count, or identifier) in src/Components/BudgetList.razor
- [ ] T054 [P] Update preview automatically when budget data changes (StateHasChanged optimization) in src/Components/BudgetPreview.razor
- [ ] T055 [P] Add responsive design for preview on small screens in wwwroot/css/preview.css
- [ ] T056 [P] Code cleanup and refactoring across all components and services
- [ ] T057 [P] Documentation updates in README.md or docs/
- [ ] T058 Run quickstart.md validation to ensure all examples work

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
  - User stories can then proceed in parallel (if staffed)
  - Or sequentially in priority order (P1 → P2)
- **Polish (Final Phase)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories
- **User Story 2 (P1)**: Can start after Foundational (Phase 2) - Depends on US1 for preview component integration
- **User Story 3 (P2)**: Can start after Foundational (Phase 2) - Depends on US1/US2 for preview to save
- **User Story 4 (P2)**: Can start after Foundational (Phase 2) - Depends on US3 for saved budgets to exist

### Within Each User Story

- Component tests (if included) MUST be written and FAIL before implementation
- Models before services
- Services before components
- Core implementation before integration
- Story complete before moving to next priority

### Parallel Opportunities

- All Setup tasks marked [P] can run in parallel
- All Foundational tasks marked [P] can run in parallel (within Phase 2)
- Once Foundational phase completes, User Stories 1 and 2 can start (US2 depends on US1 preview)
- All component tests for a user story marked [P] can run in parallel
- Models within Foundational phase marked [P] can run in parallel
- Different user stories can be worked on in parallel by different team members (with dependency awareness)

---

## Parallel Example: User Story 1

```bash
# Launch all component tests for User Story 1 together:
Task: "Component test for BudgetPreview in tests/components/BudgetPreviewTests.cs"
Task: "Component test for BudgetItemTable in tests/components/BudgetItemTableTests.cs"
Task: "Component test for ClientInfo in tests/components/ClientInfoTests.cs"

# Launch all components for User Story 1 together:
Task: "Create ClientInfo component in src/Components/ClientInfo.razor"
Task: "Create BudgetItemTable component in src/Components/BudgetItemTable.razor"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational (CRITICAL - blocks all stories)
3. Complete Phase 3: User Story 1 (Visualizar Vista Previa)
4. **STOP and VALIDATE**: Test User Story 1 independently
5. Deploy/demo if ready

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 1 → Test independently → Deploy/Demo (MVP - Preview works!)
3. Add User Story 2 → Test independently → Deploy/Demo (Calculations formatted)
4. Add User Story 3 → Test independently → Deploy/Demo (Save functionality)
5. Add User Story 4 → Test independently → Deploy/Demo (Load functionality)
6. Each story adds value without breaking previous stories

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Once Foundational is done:
   - Developer A: User Story 1 (Preview)
   - Developer B: User Story 2 (Calculations) - after US1 preview ready
   - Developer C: User Story 3 (Save) - after US1/US2 ready
3. Stories complete and integrate independently

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Verify component tests fail before implementing
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- Avoid: vague tasks, same file conflicts, cross-story dependencies that break independence
- No unit tests per constitution - only component tests and integration tests


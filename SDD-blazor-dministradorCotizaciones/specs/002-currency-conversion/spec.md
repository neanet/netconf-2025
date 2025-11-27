# Feature Specification: Conversión de Moneda y Cálculo de Presupuesto

**Feature Branch**: `002-currency-conversion`  
**Created**: 2025-01-27  
**Status**: Draft  
**Input**: User description: "quiero poder cambiar de pesos a dolares y que calcule el presupuesto"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Seleccionar Moneda para el Presupuesto (Priority: P1)

Un usuario necesita poder seleccionar la moneda (pesos o dólares) para un presupuesto. El usuario debe poder cambiar la moneda en cualquier momento y el sistema debe recalcular automáticamente todos los valores del presupuesto (precios unitarios, subtotales y totales) según la tasa de cambio configurada. La moneda seleccionada debe persistirse con el presupuesto para que se mantenga cuando se guarde y se cargue posteriormente.

**Why this priority**: Esta es la funcionalidad core de la feature. Sin la capacidad de seleccionar y cambiar la moneda, la feature no tiene valor. Es el MVP mínimo que permite a los usuarios trabajar con diferentes monedas en sus presupuestos.

**Independent Test**: Se puede probar completamente creando un presupuesto, seleccionando una moneda (pesos o dólares), agregando items con precios, cambiando la moneda, y verificando que todos los valores se recalculan correctamente según la tasa de cambio. Esto entrega valor inmediato al permitir trabajar con múltiples monedas.

**Acceptance Scenarios**:

1. **Given** el usuario está creando un nuevo presupuesto, **When** selecciona "Dólares" como moneda, **Then** el sistema muestra todos los valores monetarios (precios, subtotales, totales) en dólares con el símbolo "$" o "USD"
2. **Given** el usuario tiene un presupuesto con moneda "Pesos" y items con precios en pesos, **When** cambia la moneda a "Dólares" con una tasa de cambio de 1 USD = 50 pesos, **Then** el sistema recalcula todos los precios unitarios, subtotales y el total final en dólares (dividiendo por 50)
3. **Given** el usuario tiene un presupuesto en dólares, **When** cambia la moneda a "Pesos" con una tasa de cambio de 1 USD = 50 pesos, **Then** el sistema recalcula todos los valores multiplicando por 50
4. **Given** el usuario guarda un presupuesto con moneda "Dólares", **When** carga el presupuesto posteriormente, **Then** el sistema muestra el presupuesto con la moneda "Dólares" y todos los valores en dólares

---

### User Story 2 - Configurar Tasa de Cambio (Priority: P1)

Un usuario necesita poder configurar la tasa de cambio entre pesos y dólares para que el sistema pueda realizar las conversiones correctamente. El usuario debe poder ingresar manualmente la tasa de cambio y esta debe aplicarse a todas las conversiones del presupuesto actual.

**Why this priority**: La tasa de cambio es esencial para realizar las conversiones correctamente. Sin la capacidad de configurar la tasa de cambio, el sistema no puede convertir valores entre monedas. Es fundamental para la funcionalidad core.

**Independent Test**: Se puede probar configurando una tasa de cambio (ej: 1 USD = 50 pesos), creando un presupuesto con valores en una moneda, cambiando a la otra moneda, y verificando que los valores convertidos son correctos según la tasa configurada.

**Acceptance Scenarios**:

1. **Given** el usuario está en el editor de presupuesto, **When** configura la tasa de cambio como "1 USD = 50 pesos", **Then** el sistema guarda esta tasa y la usa para todas las conversiones
2. **Given** el usuario tiene una tasa de cambio configurada, **When** cambia la moneda del presupuesto, **Then** el sistema usa la tasa configurada para recalcular todos los valores
3. **Given** el usuario configura una tasa de cambio inválida (cero o negativa), **When** intenta guardar o usar la tasa, **Then** el sistema muestra un mensaje de error y no permite usar la tasa inválida
4. **Given** el usuario no ha configurado una tasa de cambio, **When** intenta cambiar la moneda del presupuesto, **Then** el sistema solicita que configure la tasa de cambio primero

---

### User Story 3 - Visualizar Valores en Moneda Seleccionada (Priority: P2)

Un usuario necesita ver todos los valores monetarios del presupuesto (precios unitarios, subtotales, totales) formateados correctamente según la moneda seleccionada, incluyendo el símbolo o código de moneda apropiado.

**Why this priority**: Aunque menos crítica que la conversión misma, la visualización correcta es importante para la experiencia del usuario. Sin un formato adecuado, los usuarios pueden confundirse sobre qué moneda están viendo.

**Independent Test**: Se puede probar seleccionando diferentes monedas y verificando que todos los valores se muestran con el formato y símbolo correcto de la moneda seleccionada.

**Acceptance Scenarios**:

1. **Given** el usuario tiene un presupuesto con moneda "Pesos", **When** visualiza el presupuesto, **Then** todos los valores monetarios se muestran con formato de pesos (ej: "$1,234.56" o "1,234.56 ARS")
2. **Given** el usuario tiene un presupuesto con moneda "Dólares", **When** visualiza el presupuesto, **Then** todos los valores monetarios se muestran con formato de dólares (ej: "$1,234.56" o "1,234.56 USD")
3. **Given** el usuario está viendo la vista previa del presupuesto, **When** la moneda es "Dólares", **Then** el total final se muestra claramente con el símbolo "$" o código "USD" para indicar dólares
4. **Given** el usuario cambia la moneda del presupuesto, **When** visualiza la vista previa, **Then** todos los valores se actualizan inmediatamente con el formato de la nueva moneda

---

### Edge Cases

- ¿Qué sucede cuando el usuario cambia la moneda de un presupuesto que ya tiene items con valores? El sistema debe recalcular todos los valores existentes.
- ¿Cómo maneja el sistema presupuestos guardados cuando se cambia la tasa de cambio global? Los presupuestos guardados mantienen su moneda original, pero al editarlos se puede cambiar.
- ¿Qué pasa si el usuario cambia la moneda múltiples veces? Cada cambio debe recalcular correctamente desde los valores base.
- ¿Cómo se maneja la precisión de los cálculos cuando hay conversiones con muchas decimales? El sistema debe redondear a 2 decimales para valores monetarios.
- ¿Qué sucede si la tasa de cambio es muy grande o muy pequeña? El sistema debe validar que la tasa esté en un rango razonable.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST allow users to select currency (Pesos or Dollars) for each budget
- **FR-002**: System MUST allow users to configure exchange rate between Pesos and Dollars
- **FR-003**: System MUST recalculate all monetary values (unit prices, subtotals, totals) automatically when currency is changed
- **FR-004**: System MUST persist the selected currency with the budget when saved
- **FR-005**: System MUST load and display budgets with their original currency when retrieved from storage
- **FR-006**: System MUST display all monetary values with appropriate currency symbol or code (Pesos: "$" or "ARS", Dollars: "$" or "USD")
- **FR-007**: System MUST validate that exchange rate is greater than zero before allowing conversions
- **FR-008**: System MUST round all converted monetary values to 2 decimal places
- **FR-009**: System MUST allow users to change currency at any time during budget creation or editing
- **FR-010**: System MUST maintain original values in base currency when converting, allowing conversion back without loss of precision

### Key Entities *(include if feature involves data)*

- **Currency**: Represents the selected currency for a budget (Pesos or Dollars). Stored with each budget.
- **ExchangeRate**: Represents the conversion rate between Pesos and Dollars. Configurable by user, used for all conversions in the current session.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Users can change currency and see all values recalculated in under 1 second
- **SC-002**: Currency conversion calculations maintain accuracy with 2 decimal places precision
- **SC-003**: 100% of saved budgets retain their selected currency when loaded
- **SC-004**: Users can successfully convert budgets between Pesos and Dollars without data loss
- **SC-005**: All monetary values display with correct currency formatting (symbol or code) based on selected currency

## Assumptions

- Exchange rate is manually configured by the user (not fetched from external API)
- Default currency is Pesos if not specified
- Exchange rate is stored per session or per budget, not globally
- Users understand the exchange rate they configure (1 USD = X Pesos format)
- Currency conversion is bidirectional (Pesos ↔ Dollars)
- Original values are preserved in base currency to allow accurate reconversion

## Dependencies

- Depends on existing budget calculation system (CalculationService)
- Depends on budget persistence system (BudgetService, LocalStorage)
- Requires existing budget display components (BudgetPreview, BudgetItemTable, BudgetSummary)

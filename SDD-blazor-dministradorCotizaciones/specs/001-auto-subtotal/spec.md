# Feature Specification: Cálculo Automático de Subtotal por Item

**Feature Branch**: `001-auto-subtotal`  
**Created**: 2025-01-27  
**Status**: Draft  
**Input**: User description: "Cada item debe calcular automáticamente su subtotal."

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Visualización de Subtotal Calculado Automáticamente (Priority: P1)

Un usuario está creando o editando un presupuesto y agrega un item con cantidad y precio unitario. El sistema debe calcular y mostrar automáticamente el subtotal (cantidad × precio unitario) para ese item sin que el usuario tenga que realizar el cálculo manualmente. El subtotal debe aparecer inmediatamente después de que el usuario ingresa los valores requeridos.

**Why this priority**: El cálculo automático de subtotales es fundamental para la usabilidad del sistema. Sin esta funcionalidad, los usuarios tendrían que calcular manualmente cada subtotal, lo cual es propenso a errores y reduce la eficiencia. Esta es la funcionalidad core que diferencia un sistema profesional de una hoja de cálculo básica.

**Independent Test**: Se puede probar completamente abriendo la aplicación, agregando un item con cantidad "5" y precio unitario "100.00", y verificando que el sistema muestra automáticamente el subtotal "500.00" sin intervención del usuario. Esto entrega valor inmediato al eliminar cálculos manuales.

**Acceptance Scenarios**:

1. **Given** el usuario está agregando un nuevo item a un presupuesto, **When** ingresa cantidad "10" y precio unitario "150.00", **Then** el sistema calcula y muestra automáticamente el subtotal "1,500.00" sin que el usuario haga clic en ningún botón de cálculo
2. **Given** el usuario está editando un item existente con cantidad 5 y precio 100 (subtotal 500), **When** cambia la cantidad a 8, **Then** el sistema recalcula y actualiza automáticamente el subtotal a "800.00" sin intervención del usuario
3. **Given** el usuario está editando un item existente, **When** cambia el precio unitario de 100.00 a 120.00 manteniendo cantidad 5, **Then** el sistema recalcula y actualiza automáticamente el subtotal a "600.00"
4. **Given** el usuario ingresa cantidad y precio unitario, **When** el cálculo resulta en un número con más de 2 decimales (ej: 3 × 33.333 = 99.999), **Then** el sistema redondea el subtotal a 2 decimales mostrando "100.00"

---

### User Story 2 - Actualización en Tiempo Real del Subtotal (Priority: P2)

Un usuario está editando la cantidad o precio unitario de un item y necesita ver cómo cambia el subtotal en tiempo real mientras escribe. El sistema debe actualizar el subtotal automáticamente mientras el usuario modifica los valores, proporcionando retroalimentación visual inmediata.

**Why this priority**: La actualización en tiempo real mejora significativamente la experiencia del usuario al permitirle ver inmediatamente el impacto de sus cambios. Esto es especialmente valioso cuando el usuario está ajustando valores para alcanzar un total objetivo.

**Independent Test**: Se puede probar editando un item existente, cambiando gradualmente la cantidad o precio unitario, y verificando que el subtotal se actualiza en tiempo real con cada cambio, sin necesidad de guardar o hacer clic en botones.

**Acceptance Scenarios**:

1. **Given** el usuario está editando un item con cantidad 10 y precio 100 (subtotal 1000), **When** cambia la cantidad a 15, **Then** el subtotal se actualiza inmediatamente a "1,500.00" mientras el usuario escribe
2. **Given** el usuario está editando un item, **When** borra el precio unitario y comienza a escribir un nuevo valor, **Then** el subtotal se actualiza o muestra "0.00" o un indicador de cálculo pendiente hasta que se complete el valor
3. **Given** el usuario está editando un item con valores válidos, **When** ingresa temporalmente un valor inválido (ej: texto en campo numérico), **Then** el sistema muestra un mensaje de error pero mantiene el último subtotal válido calculado

---

### User Story 3 - Precisión y Formato del Subtotal (Priority: P3)

Un usuario necesita que los subtotales se calculen con precisión matemática y se muestren en formato monetario estándar. El sistema debe manejar correctamente números grandes, decimales, y casos límite sin errores de redondeo que afecten la precisión del presupuesto.

**Why this priority**: Aunque menos visible que el cálculo automático, la precisión es crítica para la confiabilidad del sistema. Errores de redondeo o formato incorrecto pueden llevar a discrepancias en los totales y pérdida de confianza del usuario.

**Independent Test**: Se puede probar creando items con valores que produzcan cálculos complejos (números grandes, muchos decimales) y verificando que los subtotales son matemáticamente correctos y están formateados apropiadamente como valores monetarios.

**Acceptance Scenarios**:

1. **Given** el usuario ingresa cantidad 999.99 y precio unitario 1234.56, **When** el sistema calcula el subtotal, **Then** muestra "1,234,455.44" (correctamente calculado y formateado con separadores de miles y 2 decimales)
2. **Given** el usuario ingresa cantidad 1 y precio unitario 0.01, **When** el sistema calcula el subtotal, **Then** muestra "0.01" (mantiene precisión para valores pequeños)
3. **Given** el usuario ingresa valores que resultan en un cálculo con más de 2 decimales, **When** el sistema calcula el subtotal, **Then** redondea a 2 decimales usando reglas estándar de redondeo bancario (redondeo a par para .5)

---

### Edge Cases

- ¿Qué sucede cuando el usuario ingresa cantidad 0? El sistema debe calcular subtotal como 0.00 (aunque la validación puede rechazar cantidad 0 según otras reglas de negocio)
- ¿Qué sucede cuando el usuario ingresa precio unitario 0? El sistema debe calcular subtotal como 0.00 (puede ser un item gratuito)
- ¿Qué sucede cuando el usuario borra la cantidad o precio unitario temporalmente? El sistema debe manejar valores vacíos mostrando 0.00 o un indicador apropiado hasta que se complete el valor
- ¿Qué sucede cuando el cálculo resulta en un número extremadamente grande? El sistema debe formatear correctamente con separadores de miles y manejar notación científica si es necesario
- ¿Qué sucede cuando hay un desbordamiento numérico en el cálculo? El sistema debe detectar y mostrar un error apropiado en lugar de mostrar un valor incorrecto
- ¿Qué sucede cuando el usuario ingresa valores que resultan en precisión de punto flotante (ej: 0.1 + 0.2)? El sistema debe usar aritmética de precisión decimal para evitar errores de redondeo
- ¿Qué sucede cuando el subtotal calculado es exactamente 0.00? El sistema debe mostrar "0.00" claramente, no un campo vacío

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST automatically calculate the subtotal for each item as quantity × unit price
- **FR-002**: System MUST display the calculated subtotal immediately after the user enters both quantity and unit price
- **FR-003**: System MUST update the subtotal automatically in real-time when the user modifies either quantity or unit price
- **FR-004**: System MUST recalculate the subtotal automatically when editing an existing item
- **FR-005**: System MUST format subtotals as monetary values with exactly 2 decimal places
- **FR-006**: System MUST use proper rounding rules (banker's rounding for .5 cases) when subtotal has more than 2 decimal places
- **FR-007**: System MUST handle edge cases gracefully: zero values (0.00), empty fields (show 0.00 or pending indicator), and very large numbers (proper formatting)
- **FR-008**: System MUST use decimal precision arithmetic to avoid floating-point calculation errors
- **FR-009**: System MUST display subtotal even when quantity or unit price is zero (showing 0.00)
- **FR-010**: System MUST update subtotal within 100ms of user input change for real-time feedback
- **FR-011**: System MUST handle numeric overflow scenarios by detecting and displaying appropriate error messages
- **FR-012**: System MUST maintain calculation precision for very small values (e.g., 0.01)

### Key Entities *(include if feature involves data)*

- **Item**: Represents a line item in a budget. For this feature, the key attribute is the calculated subtotal (quantity × unit price). The subtotal is a derived/calculated attribute that must be computed automatically and cannot be manually edited by the user. The subtotal is formatted as a monetary value with 2 decimal places.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: The system calculates and displays subtotals automatically within 100ms of user entering both quantity and unit price
- **SC-002**: 100% of subtotal calculations are mathematically correct when tested with 1000 different quantity/price combinations
- **SC-003**: Users can see subtotal updates in real-time (within 100ms) when modifying quantity or unit price values
- **SC-004**: All subtotals are formatted consistently as monetary values with exactly 2 decimal places, regardless of input values
- **SC-005**: The system handles edge cases (zero values, empty fields, large numbers) without calculation errors or display issues
- **SC-006**: Subtotal calculations maintain precision for values ranging from 0.01 to 999,999,999.99 without rounding errors
- **SC-007**: Users report that subtotals appear "instantly" or "automatically" without needing to trigger calculation manually (qualitative feedback)

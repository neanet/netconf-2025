# Feature Specification: Crear Presupuestos con Items

**Feature Branch**: `001-create-budget`  
**Created**: 2025-01-27  
**Status**: Draft  
**Input**: User description: "La aplicación debe permitir a usuarios crear presupuestos profesionales agregando items con descripción, cantidad y precio unitario."

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Crear Presupuesto Nuevo y Agregar Items (Priority: P1)

Un usuario necesita crear un presupuesto profesional desde cero agregando items con información básica. El usuario inicia con una pantalla en blanco y puede agregar múltiples items, cada uno con descripción, cantidad y precio unitario. El sistema calcula automáticamente el total de cada item y el total general del presupuesto. El usuario puede ver el presupuesto en tiempo real mientras agrega items.

**Why this priority**: Esta es la funcionalidad core del sistema. Sin la capacidad de crear presupuestos y agregar items, la aplicación no tiene valor. Es el MVP mínimo viable que permite a los usuarios realizar su tarea principal.

**Independent Test**: Se puede probar completamente abriendo la aplicación, creando un nuevo presupuesto, agregando al menos un item con descripción, cantidad y precio unitario, y verificando que el sistema calcula y muestra los totales correctamente. Esto entrega valor inmediato al usuario.

**Acceptance Scenarios**:

1. **Given** el usuario abre la aplicación, **When** hace clic en "Nuevo Presupuesto", **Then** se muestra un formulario vacío listo para agregar items
2. **Given** el usuario está en un presupuesto nuevo, **When** agrega un item con descripción "Servicio de consultoría", cantidad "10" y precio unitario "150.00", **Then** el sistema muestra el item en la lista con total calculado "1,500.00" y actualiza el total general
3. **Given** el usuario tiene un presupuesto con items, **When** agrega un segundo item con descripción "Desarrollo de software", cantidad "5" y precio unitario "200.00", **Then** el sistema muestra ambos items, calcula el total del segundo item como "1,000.00" y actualiza el total general a "2,500.00"
4. **Given** el usuario está agregando un item, **When** intenta guardar sin completar todos los campos requeridos (descripción, cantidad, precio unitario), **Then** el sistema muestra mensajes de error específicos indicando qué campos faltan

---

### User Story 2 - Editar Items Existentes (Priority: P2)

Un usuario necesita modificar items que ya agregó a un presupuesto. El usuario puede editar la descripción, cantidad o precio unitario de cualquier item existente, y el sistema recalcula automáticamente los totales afectados.

**Why this priority**: Los usuarios frecuentemente necesitan ajustar información después de agregarla. Sin esta capacidad, los usuarios tendrían que eliminar y recrear items, lo cual es ineficiente y frustrante.

**Independent Test**: Se puede probar creando un presupuesto con al menos un item, luego editando cualquiera de sus campos (descripción, cantidad o precio unitario) y verificando que los cambios se reflejan correctamente y los totales se recalculan.

**Acceptance Scenarios**:

1. **Given** el usuario tiene un presupuesto con un item "Servicio A" cantidad 10 precio 150, **When** edita la cantidad a 15, **Then** el total del item se actualiza a 2,250.00 y el total general se recalcula
2. **Given** el usuario tiene un presupuesto con múltiples items, **When** edita el precio unitario de un item, **Then** solo el total de ese item y el total general se actualizan, los demás items permanecen sin cambios
3. **Given** el usuario está editando un item, **When** intenta guardar con valores inválidos (cantidad negativa o precio unitario negativo), **Then** el sistema muestra un mensaje de error y no guarda los cambios

---

### User Story 3 - Eliminar Items de un Presupuesto (Priority: P3)

Un usuario necesita eliminar items que agregó por error o que ya no son necesarios en el presupuesto. El usuario puede eliminar cualquier item y el sistema actualiza automáticamente el total general.

**Why this priority**: Aunque menos frecuente que agregar o editar, la capacidad de eliminar items es esencial para mantener presupuestos limpios y corregir errores. Sin embargo, es menos crítica que las funcionalidades anteriores.

**Independent Test**: Se puede probar creando un presupuesto con múltiples items, eliminando uno de ellos, y verificando que el item desaparece de la lista y el total general se actualiza correctamente.

**Acceptance Scenarios**:

1. **Given** el usuario tiene un presupuesto con 3 items con total general de 5,000.00, **When** elimina un item con total de 1,000.00, **Then** el item desaparece de la lista y el total general se actualiza a 4,000.00
2. **Given** el usuario está eliminando un item, **When** confirma la eliminación, **Then** el sistema muestra un mensaje de confirmación antes de eliminar y luego actualiza la lista
3. **Given** el usuario tiene un presupuesto con un solo item, **When** elimina ese item, **Then** el presupuesto queda vacío con total general en 0.00 y permite agregar nuevos items

---

### Edge Cases

- ¿Qué sucede cuando el usuario ingresa una cantidad de 0? El sistema debe rechazar cantidades de 0 o negativas
- ¿Qué sucede cuando el usuario ingresa un precio unitario de 0? El sistema debe permitir precio 0 (puede ser un item gratuito o muestra) pero debe advertir al usuario
- ¿Qué sucede cuando el usuario ingresa valores muy grandes (ej: cantidad 999999)? El sistema debe validar límites razonables y mostrar advertencias si es necesario
- ¿Qué sucede cuando el usuario ingresa descripciones muy largas? El sistema debe permitir descripciones largas pero puede truncar en la visualización si es necesario
- ¿Qué sucede cuando el usuario intenta editar un presupuesto que no tiene items? El sistema debe mostrar un mensaje indicando que no hay items para editar
- ¿Qué sucede cuando el cálculo de totales resulta en números con muchos decimales? El sistema debe redondear a 2 decimales para montos monetarios
- ¿Qué sucede si el usuario cierra la aplicación sin guardar? El sistema debe guardar automáticamente en Local Storage según la constitución del proyecto

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST allow users to create a new budget/presupuesto from scratch
- **FR-002**: System MUST allow users to add items to a budget with the following required fields: description (descripción), quantity (cantidad), and unit price (precio unitario)
- **FR-003**: System MUST automatically calculate the line total for each item (quantity × unit price)
- **FR-004**: System MUST automatically calculate and display the grand total of all items in the budget
- **FR-005**: System MUST validate that quantity is a positive number greater than zero
- **FR-006**: System MUST validate that unit price is a non-negative number (zero allowed for free items)
- **FR-007**: System MUST validate that description is not empty or whitespace-only
- **FR-008**: System MUST allow users to edit any existing item's description, quantity, or unit price
- **FR-009**: System MUST recalculate totals automatically when an item is edited
- **FR-010**: System MUST allow users to delete items from a budget
- **FR-011**: System MUST recalculate the grand total automatically when an item is deleted
- **FR-012**: System MUST display error messages when validation fails, indicating which field has the error
- **FR-013**: System MUST persist all budget data (presupuestos and items) in Local Storage automatically
- **FR-014**: System MUST load existing budgets from Local Storage when the application starts
- **FR-015**: System MUST format monetary values with 2 decimal places
- **FR-016**: System MUST handle edge cases gracefully (empty budgets, single item, etc.)

### Key Entities *(include if feature involves data)*

- **Presupuesto (Budget)**: Represents a professional quote/estimate document. Key attributes: unique identifier, creation date, last modified date, list of items, grand total. A presupuesto contains zero or more items.

- **Item**: Represents a line item in a budget. Key attributes: unique identifier within the budget, description (text), quantity (positive number), unit price (non-negative number), line total (calculated: quantity × unit price). Each item belongs to exactly one presupuesto.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Users can create a new budget and add their first item with all required fields in under 30 seconds
- **SC-002**: Users can successfully add 10 items to a budget without errors or data loss
- **SC-003**: 95% of users can complete the task of creating a budget with at least 3 items on their first attempt without needing help
- **SC-004**: The system correctly calculates totals for budgets with up to 100 items without performance degradation
- **SC-005**: All budget data persists correctly in Local Storage and is available after browser refresh or application restart
- **SC-006**: Users receive immediate visual feedback (within 100ms) when adding, editing, or deleting items
- **SC-007**: Validation errors are displayed within 500ms of user attempting to save invalid data

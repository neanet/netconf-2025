# Feature Specification: Vista Previa Profesional y Guardado/Carga de Presupuestos

**Feature Branch**: `001-budget-preview`  
**Created**: 2025-01-27  
**Status**: Draft  
**Input**: User description: "Necesito una vista previa del presupuesto con formato profesional que muestre todos los items, cálculos intermedios y el total final. Los presupuestos deben poder guardarse y cargarse desde el LocalStorage del navegador."

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Visualizar Vista Previa Profesional del Presupuesto (Priority: P1)

Un usuario necesita ver una vista previa del presupuesto con formato profesional antes de generarlo o enviarlo. La vista previa debe mostrar toda la información del presupuesto de manera clara y organizada: información del cliente, todos los items con sus detalles (descripción, cantidad, precio unitario, subtotal), y el total final. El formato debe ser limpio, legible y profesional, similar a cómo se vería el documento final.

**Why this priority**: La vista previa es esencial para que el usuario pueda verificar que toda la información está correcta antes de finalizar el presupuesto. Sin una vista previa clara, los usuarios no pueden validar sus datos y pueden generar documentos con errores. Es fundamental para la confianza y calidad del sistema.

**Independent Test**: Se puede probar completamente creando un presupuesto con al menos 2 items, información del cliente, y verificando que la vista previa muestra todos los elementos de forma clara y profesional: información del cliente, tabla de items con columnas (descripción, cantidad, precio unitario, subtotal), y total final destacado. Esto entrega valor inmediato al permitir verificación visual completa.

**Acceptance Scenarios**:

1. **Given** el usuario tiene un presupuesto con información del cliente y 3 items, **When** visualiza la vista previa, **Then** el sistema muestra claramente: nombre y empresa del cliente, email, una tabla con los 3 items mostrando descripción, cantidad, precio unitario y subtotal de cada uno, y el total final destacado
2. **Given** el usuario tiene un presupuesto con 10 items, **When** visualiza la vista previa, **Then** el sistema muestra todos los 10 items en una tabla organizada con scroll si es necesario, manteniendo encabezados visibles
3. **Given** el usuario está visualizando la vista previa, **When** el presupuesto tiene información del cliente completa, **Then** el sistema muestra la información del cliente en un formato destacado al inicio de la vista previa
4. **Given** el usuario está visualizando la vista previa, **When** el presupuesto no tiene información del cliente, **Then** el sistema muestra un indicador o sección vacía para la información del cliente, pero aún muestra los items y totales

---

### User Story 2 - Mostrar Cálculos Intermedios y Total Final (Priority: P1)

Un usuario necesita ver claramente los cálculos intermedios (subtotal de cada item) y el total final del presupuesto en la vista previa. Cada item debe mostrar su subtotal calculado (cantidad × precio unitario), y el total final debe ser la suma de todos los subtotales. Los valores monetarios deben estar formateados correctamente con separadores de miles y 2 decimales.

**Why this priority**: Los cálculos son críticos para la verificación del presupuesto. El usuario debe poder verificar que los cálculos son correctos antes de enviar el presupuesto. Sin mostrar los cálculos intermedios, el usuario no puede validar la precisión de los totales.

**Independent Test**: Se puede probar creando un presupuesto con items que tengan diferentes cantidades y precios, y verificando que cada subtotal se muestra correctamente (ej: cantidad 5 × precio 100 = subtotal 500), y que el total final es la suma correcta de todos los subtotales. Esto entrega valor al permitir verificación matemática.

**Acceptance Scenarios**:

1. **Given** el usuario tiene un presupuesto con item 1 (cantidad 10, precio 150) e item 2 (cantidad 5, precio 200), **When** visualiza la vista previa, **Then** el sistema muestra subtotal del item 1 como "1,500.00", subtotal del item 2 como "1,000.00", y total final como "2,500.00"
2. **Given** el usuario está visualizando la vista previa, **When** el presupuesto tiene items con valores que resultan en decimales, **Then** el sistema muestra todos los subtotales y el total final formateados con exactamente 2 decimales
3. **Given** el usuario está visualizando la vista previa, **When** el presupuesto tiene un solo item, **Then** el sistema muestra el subtotal de ese item y el total final (que será igual al subtotal)
4. **Given** el usuario está visualizando la vista previa, **When** el presupuesto no tiene items, **Then** el sistema muestra el total final como "0.00" o un mensaje indicando que no hay items

---

### User Story 3 - Guardar Presupuesto en LocalStorage (Priority: P2)

Un usuario necesita guardar el presupuesto en LocalStorage del navegador para poder acceder a él más tarde. El sistema debe guardar automáticamente el presupuesto cuando se crea o modifica, y también debe permitir guardado manual. El usuario debe recibir confirmación visual cuando el presupuesto se guarda exitosamente.

**Why this priority**: El guardado es esencial para la persistencia de datos. Sin guardado, los usuarios perderían su trabajo si cierran el navegador o recargan la página. Aunque menos visible que la vista previa, es fundamental para la funcionalidad básica del sistema.

**Independent Test**: Se puede probar creando o editando un presupuesto, verificando que se guarda automáticamente en LocalStorage, y luego recargando la página para confirmar que el presupuesto está disponible. Esto entrega valor al preservar el trabajo del usuario.

**Acceptance Scenarios**:

1. **Given** el usuario crea un nuevo presupuesto con información del cliente y 2 items, **When** el sistema guarda automáticamente, **Then** el presupuesto está disponible en LocalStorage y persiste después de recargar la página
2. **Given** el usuario está editando un presupuesto existente, **When** modifica un item y el sistema guarda automáticamente, **Then** los cambios están guardados y disponibles después de recargar
3. **Given** el usuario tiene un presupuesto guardado, **When** cierra y vuelve a abrir el navegador, **Then** el presupuesto sigue disponible y se puede cargar
4. **Given** el usuario intenta guardar un presupuesto, **When** LocalStorage está lleno o no disponible, **Then** el sistema muestra un mensaje de error apropiado al usuario

---

### User Story 4 - Cargar Presupuestos desde LocalStorage (Priority: P2)

Un usuario necesita poder cargar presupuestos previamente guardados desde LocalStorage. El sistema debe mostrar una lista de presupuestos guardados y permitir al usuario seleccionar uno para cargarlo. Una vez cargado, el usuario debe poder ver y editar el presupuesto completo.

**Why this priority**: La capacidad de cargar presupuestos guardados es esencial para continuar trabajando en presupuestos existentes. Sin esta funcionalidad, los usuarios no podrían acceder a su trabajo previo, limitando severamente la utilidad del sistema.

**Independent Test**: Se puede probar guardando al menos 2 presupuestos diferentes, luego abriendo la aplicación y verificando que ambos presupuestos aparecen en una lista, y que al seleccionar uno se carga correctamente con toda su información (cliente, items, totales). Esto entrega valor al permitir continuidad del trabajo.

**Acceptance Scenarios**:

1. **Given** el usuario tiene 3 presupuestos guardados en LocalStorage, **When** abre la aplicación, **Then** el sistema muestra una lista con los 3 presupuestos identificados por información del cliente o fecha de creación
2. **Given** el usuario está viendo la lista de presupuestos guardados, **When** selecciona un presupuesto, **Then** el sistema carga el presupuesto completo con toda su información (cliente, items, totales) y muestra la vista previa
3. **Given** el usuario carga un presupuesto guardado, **When** el presupuesto tiene información del cliente y 5 items, **Then** el sistema muestra correctamente toda la información del cliente y los 5 items con sus subtotales y total final
4. **Given** el usuario intenta cargar un presupuesto, **When** el archivo en LocalStorage está corrupto o en formato inválido, **Then** el sistema muestra un mensaje de error y no carga datos inválidos

---

### Edge Cases

- ¿Qué sucede cuando el usuario tiene muchos presupuestos guardados (ej: 50+)? El sistema debe manejar la lista eficientemente, posiblemente con paginación o búsqueda
- ¿Qué sucede cuando el usuario intenta guardar un presupuesto muy grande (muchos items)? El sistema debe manejar presupuestos grandes dentro de los límites de LocalStorage (típicamente 5-10MB)
- ¿Qué sucede cuando LocalStorage está lleno? El sistema debe detectar esto y ofrecer opciones (eliminar presupuestos antiguos, exportar, etc.)
- ¿Qué sucede cuando el usuario tiene presupuestos guardados en un navegador y abre la aplicación en otro navegador? El sistema debe indicar que no hay presupuestos guardados (LocalStorage es específico del navegador)
- ¿Qué sucede cuando el usuario borra datos del navegador? El sistema debe manejar gracefully la ausencia de datos guardados
- ¿Qué sucede cuando la vista previa tiene muchos items y la pantalla es pequeña? El sistema debe permitir scroll o paginación para ver todos los items
- ¿Qué sucede cuando el usuario modifica un presupuesto cargado? El sistema debe guardar los cambios automáticamente o permitir guardado manual
- ¿Qué sucede cuando hay múltiples presupuestos con el mismo nombre de cliente? El sistema debe diferenciarlos (ej: por fecha, número de items, o identificador único)

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST display a professional preview of the budget showing all items, intermediate calculations, and final total
- **FR-002**: System MUST show client information (name, company, email) prominently in the preview
- **FR-003**: System MUST display all items in a clear table format with columns: description, quantity, unit price, and subtotal
- **FR-004**: System MUST show the subtotal for each item (calculated as quantity × unit price) in the preview
- **FR-005**: System MUST display the final total (sum of all item subtotals) prominently in the preview
- **FR-006**: System MUST format all monetary values with 2 decimal places and thousand separators in the preview
- **FR-007**: System MUST automatically save budgets to LocalStorage when created or modified
- **FR-008**: System MUST allow manual saving of budgets to LocalStorage
- **FR-009**: System MUST provide visual confirmation when a budget is successfully saved
- **FR-010**: System MUST load all saved budgets from LocalStorage when the application starts
- **FR-011**: System MUST display a list of saved budgets allowing users to select and load one
- **FR-012**: System MUST load a selected budget with all its data (client information, items, calculations) correctly
- **FR-013**: System MUST handle cases where LocalStorage is full or unavailable gracefully with appropriate error messages
- **FR-014**: System MUST preserve all budget data (client info, items, totals) when saving and loading
- **FR-015**: System MUST handle corrupted or invalid data in LocalStorage without crashing the application
- **FR-016**: System MUST allow the preview to scroll or paginate when there are many items
- **FR-017**: System MUST differentiate between multiple budgets with similar client information (e.g., by date, identifier, or item count)
- **FR-018**: System MUST update the preview automatically when budget data changes

### Key Entities *(include if feature involves data)*

- **Presupuesto (Budget)**: Represents a complete budget document that can be previewed, saved, and loaded. Key attributes: unique identifier, client information, list of items, calculated totals, creation date, last modified date. The presupuesto is the complete entity that is saved to and loaded from LocalStorage. The preview displays all attributes of a presupuesto in a professional format.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Users can view a complete budget preview with all items and calculations in under 2 seconds after opening or loading a budget
- **SC-002**: 100% of saved budgets load correctly with all data intact (client info, items, totals) when tested with 100 different budget configurations
- **SC-003**: 95% of users can successfully save and load a budget on their first attempt without needing help
- **SC-004**: All monetary values in the preview are formatted consistently with 2 decimal places and thousand separators
- **SC-005**: The preview displays correctly for budgets with up to 100 items without performance degradation
- **SC-006**: Budgets are automatically saved within 1 second of any modification
- **SC-007**: Users can load a previously saved budget and see the complete preview in under 3 seconds
- **SC-008**: The system handles LocalStorage full scenarios gracefully, providing clear error messages and recovery options

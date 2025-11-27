<!--
Sync Impact Report:
Version change: 2.0.0 → 2.1.0 (MINOR - modificación material de principio de testing)
Modified principles: 
  - III. Test-First Development → III. Test-First Development (eliminadas pruebas unitarias, mantenidos tests de componentes e integración)
Added sections: N/A
Removed sections: N/A
Templates requiring updates:
  ✅ plan-template.md - Constitution Check section compatible (se llena dinámicamente)
  ✅ spec-template.md - No direct constitution references, compatible
  ✅ tasks-template.md - Actualizado: eliminada referencia a unit tests, cambiada a component tests
  ✅ checklist-template.md - No direct constitution references, compatible
Follow-up TODOs: None
-->

# Generador de Presupuestos/Cotizaciones Constitution

## Core Principles

### I. Arquitectura Blazor WebAssembly Standalone

La aplicación DEBE ser una aplicación Blazor WebAssembly standalone ejecutándose completamente en el navegador sin dependencia de servidor backend. Toda la lógica de negocio, validación, generación de documentos y persistencia DEBE ejecutarse en el cliente (WASM). Los componentes Blazor DEBEN ser reutilizables y encapsular su propia lógica de presentación. La persistencia de datos DEBE realizarse mediante Local Storage del navegador mediante servicios abstraídos e inyectables.

**Rationale**: Una aplicación standalone Blazor WebAssembly elimina la necesidad de infraestructura de servidor, reduce costos operativos y permite funcionamiento offline completo. Local Storage proporciona persistencia local sin requerir servidor, ideal para aplicaciones de alcance inicial.

### II. Componentes Reutilizables y Modularidad

Cada funcionalidad DEBE implementarse como componente Blazor independiente y reutilizable. Los componentes DEBEN seguir el principio de responsabilidad única y exponer interfaces claras mediante parámetros y eventos. La lógica de negocio DEBE separarse de los componentes UI mediante servicios inyectables. Los modelos de datos DEBEN estar centralizados en proyectos de biblioteca compartida dentro de la solución.

**Rationale**: La modularidad facilita el mantenimiento, las pruebas y la evolución del sistema. Los componentes reutilizables reducen la duplicación de código y mejoran la consistencia de la UI. La separación mediante servicios inyectables permite testing independiente y evolución del sistema.

### III. Test-First Development (NON-NEGOTIABLE)

El desarrollo DEBE seguir TDD estrictamente: Tests escritos → Aprobación del usuario → Tests fallan → Implementación. El ciclo Red-Green-Refactor DEBE aplicarse para todas las funcionalidades. Los tests de componentes Blazor DEBEN validar el comportamiento de la UI. Los tests de integración DEBEN validar la persistencia en Local Storage y la generación de documentos en el cliente.

**Rationale**: TDD garantiza que el código cumple con los requisitos desde el inicio y facilita la refactorización segura. Los tests actúan como documentación ejecutable del comportamiento esperado. En una aplicación standalone, los tests de integración validan la interacción entre servicios y Local Storage. Los tests de componentes validan el comportamiento de la interfaz de usuario.

### IV. Persistencia Local Storage

La persistencia de datos DEBE realizarse mediante Local Storage del navegador. El acceso a Local Storage DEBE abstraerse mediante un servicio inyectable para facilitar pruebas y mantenimiento. Los datos almacenados localmente DEBEN incluir: presupuestos, cotizaciones, clientes, productos, configuración de usuario y preferencias. El sistema DEBE manejar casos donde Local Storage no esté disponible (modo privado, cuota excedida) degradando gracefully con mensajes informativos al usuario. Los datos críticos DEBEN validarse antes de persistir y el sistema DEBE proporcionar mecanismos de exportación/importación para respaldo.

**Restricciones de Local Storage**: Los datos almacenados NO DEBEN incluir información sensible sin encriptar cuando sea posible. Los datos DEBEN tener estructura versionada para permitir migración futura. El sistema DEBE implementar límites de tamaño y limpieza de datos antiguos para evitar exceder cuotas del navegador.

**Rationale**: Local Storage permite persistencia local sin servidor, ideal para el alcance inicial del proyecto. La abstracción mediante servicios facilita el testing y permite migración futura a otras formas de persistencia si es necesario. Los mecanismos de exportación/importación proporcionan respaldo y portabilidad de datos.

### V. Generación de Documentos PDF en Cliente

La generación de presupuestos y cotizaciones en formato PDF DEBE realizarse completamente en el cliente (WASM) utilizando bibliotecas compatibles con Blazor WebAssembly. Los templates de documentos DEBEN ser configurables y reutilizables. La generación DEBE soportar múltiples formatos de salida (PDF, HTML, impresión). Los documentos generados DEBEN incluir metadatos y versionado para trazabilidad. La generación DEBE ser eficiente y no bloquear la UI durante el proceso.

**Rationale**: La generación en cliente elimina la dependencia de servidor y permite funcionamiento completamente offline. Las bibliotecas modernas de .NET 10 permiten generación de PDF eficiente en WASM. Los templates configurables permiten personalización sin cambios de código.

### VI. Validación y Seguridad de Datos

Todos los datos de entrada DEBEN validarse en el cliente antes de persistir. La validación DEBE proporcionar retroalimentación inmediata al usuario mediante mensajes claros y específicos. Los datos DEBEN validarse utilizando Data Annotations de .NET y validación personalizada cuando sea necesario. Los datos sensibles almacenados en Local Storage DEBEN considerarse como datos locales del usuario y el sistema DEBE informar claramente sobre las limitaciones de seguridad del almacenamiento local. Los presupuestos y cotizaciones DEBEN incluir auditoría de cambios con registro de autor, fecha y motivo.

**Rationale**: La validación robusta en cliente previene errores de entrada y mejora la experiencia del usuario. Aunque Local Storage no es tan seguro como un servidor, para el alcance inicial proporciona funcionalidad adecuada. La auditoría de cambios es esencial para trazabilidad en procesos comerciales.

### VII. Versionado y Gestión de Cambios

El sistema DEBE mantener historial de versiones de presupuestos y cotizaciones. Cada modificación DEBE registrar autor, fecha y motivo del cambio. Los documentos generados DEBEN incluir número de versión visible. El sistema DEBE permitir comparar versiones y restaurar versiones anteriores. Los cambios en la estructura de datos DEBEN seguir versionado semántico (MAJOR.MINOR.PATCH).

**Rationale**: El versionado es esencial para la trazabilidad en procesos comerciales. Permite auditoría y resolución de disputas. Facilita la evolución del sistema sin romper compatibilidad.

## Technology Stack

### Stack Principal

- **Framework**: .NET 10 (exclusivamente)
- **Frontend**: Blazor WebAssembly
- **Lenguaje**: C# 13
- **Testing**: bUnit (para componentes Blazor), tests de integración
- **Generación PDF**: QuestPDF o bibliotecas compatibles con Blazor WASM (evaluar según requisitos de .NET 10)
- **Persistencia**: Local Storage del navegador mediante servicios abstraídos
- **Logging**: Microsoft.Extensions.Logging (nativo de .NET)

### Dependencias Principales (.NET 10)

- Microsoft.AspNetCore.Components.WebAssembly
- Microsoft.AspNetCore.Components.WebAssembly.DevServer
- Blazored.LocalStorage o Microsoft.JSInterop (para acceso a Local Storage)
- Microsoft.Extensions.DependencyInjection (inyección de dependencias)
- System.Text.Json (serialización JSON nativa)

### Restricciones de Tecnología

- **Solo .NET 10**: El proyecto DEBE usar exclusivamente tecnologías y bibliotecas compatibles con .NET 10. No se permiten dependencias de versiones anteriores de .NET a menos que sean compatibles con .NET 10.
- **Sin Backend**: No se utilizará servidor backend, API, ni bases de datos servidor. Toda la funcionalidad DEBE ejecutarse en el cliente.
- **Sin Node.js requerido**: El desarrollo DEBE poder realizarse solo con .NET 10 SDK. Node.js es opcional solo para herramientas de desarrollo.

### Restricciones de Plataforma

- **Target Platform**: Navegadores modernos con soporte WebAssembly (Chrome, Firefox, Edge, Safari)
- **Requisitos**: .NET 10 SDK únicamente
- **Despliegue**: Hosting estático para WASM (Azure Static Web Apps, GitHub Pages, Netlify, etc.)

## Development Workflow

### Flujo de Desarrollo

1. **Especificación**: Crear spec.md con user stories priorizadas (P1, P2, P3...)
2. **Planificación**: Generar plan.md con investigación técnica y diseño
3. **Tareas**: Descomponer en tasks.md organizadas por user story
4. **Implementación**: Seguir TDD estrictamente (tests → implementación)
5. **Revisión**: Verificar cumplimiento con constitución antes de merge
6. **Despliegue**: Validar en entorno de staging antes de producción

### Code Review Requirements

- Verificar cumplimiento con principios de la constitución
- Validar que los tests cubren la funcionalidad implementada
- Revisar separación de responsabilidades (componentes, servicios, modelos)
- Verificar manejo de errores y validación de datos
- Confirmar que el acceso a Local Storage está abstraído mediante servicios
- Verificar que solo se usan tecnologías de .NET 10

### Quality Gates

- Todos los tests DEBEN pasar antes de merge
- No se permiten violaciones de principios sin justificación documentada
- Los componentes Blazor DEBEN tener tests de UI
- Los servicios de persistencia DEBEN tener tests de integración con Local Storage mockeado
- Verificar que no se introducen dependencias de backend o bases de datos servidor

## Governance

Esta constitución SUPERA todas las demás prácticas y decisiones técnicas del proyecto. Las decisiones que violen estos principios REQUIEREN justificación explícita y aprobación del equipo.

### Amendment Procedure

Las modificaciones a esta constitución DEBEN:
1. Documentarse con razón de cambio clara
2. Obtener aprobación del equipo mediante revisión
3. Incluir plan de migración si hay cambios incompatibles
4. Actualizar versionado semántico (MAJOR.MINOR.PATCH)
5. Sincronizar todos los templates y documentos dependientes

### Compliance Review

- Todos los PRs/reviews DEBEN verificar cumplimiento con la constitución
- La complejidad adicional DEBE justificarse documentando alternativas rechazadas
- Las violaciones DEBEN documentarse en el Complexity Tracking del plan.md
- Revisión trimestral de cumplimiento y efectividad de principios

### Versioning Policy

- **MAJOR**: Cambios incompatibles en principios o eliminación de principios
- **MINOR**: Nuevos principios agregados o expansión material de guía existente
- **PATCH**: Clarificaciones, correcciones de redacción, refinamientos no semánticos

**Version**: 2.1.0 | **Ratified**: 2025-01-27 | **Last Amended**: 2025-01-27

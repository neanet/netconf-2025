# BudgetApp - Sistema de Gestión de Presupuestos

Aplicación web Blazor WebAssembly para la creación, gestión y visualización de presupuestos profesionales. Permite crear presupuestos con información de clientes, múltiples items, cálculos automáticos y conversión de monedas.

## 🚀 Características Principales

- **Creación de Presupuestos**: Crea presupuestos con información completa del cliente y múltiples items
- **Vista Previa Profesional**: Visualiza el presupuesto con formato profesional antes de finalizarlo
- **Cálculos Automáticos**: Cálculo automático de subtotales y totales con formato monetario
- **Persistencia Local**: Guardado y carga de presupuestos en LocalStorage del navegador
- **Conversión de Monedas**: Soporte para múltiples monedas con tasas de cambio configurables
- **Auto-guardado**: Guardado automático de cambios en los presupuestos
- **Interfaz Moderna**: UI construida con Blazor y Bootstrap

## 📋 Requisitos

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) o superior
- Navegador web moderno (Chrome, Firefox, Edge, Safari)

## 🛠️ Instalación y Ejecución

### Clonar el Repositorio

```bash
git clone <url-del-repositorio>
cd spec-kit-template-cursor-agent-ps-v0.0.85
```

### Restaurar Dependencias

```bash
dotnet restore
```

### Ejecutar la Aplicación

```bash
dotnet run
```

La aplicación se ejecutará en `https://localhost:5001` (o el puerto configurado en `Properties/launchSettings.json`).

### Compilar para Producción

```bash
dotnet build -c Release
```

## 🧪 Ejecutar Tests

El proyecto incluye tests unitarios y de integración:

```bash
dotnet test
```

### Tests Incluidos

- **BudgetPreviewTests**: Tests del componente de vista previa de presupuestos
- **CalculationServiceTests**: Tests del servicio de cálculos
- **BudgetPersistenceTests**: Tests de integración para persistencia de presupuestos

## 📁 Estructura del Proyecto

```
BudgetApp/
├── src/
│   ├── Components/          # Componentes Blazor reutilizables
│   │   ├── BudgetPreview.razor
│   │   ├── BudgetItemList.razor
│   │   ├── BudgetSummary.razor
│   │   ├── ClientInfo.razor
│   │   └── ...
│   ├── Models/              # Modelos de datos
│   │   ├── Budget.cs
│   │   ├── BudgetItem.cs
│   │   ├── Client.cs
│   │   └── Currency.cs
│   ├── Pages/               # Páginas de la aplicación
│   │   ├── BudgetEditor.razor
│   │   ├── BudgetList.razor
│   │   └── Home.razor
│   ├── Services/            # Servicios de negocio
│   │   ├── BudgetService.cs
│   │   ├── CalculationService.cs
│   │   ├── CurrencyService.cs
│   │   └── LocalStorageService.cs
│   └── Layout/              # Layouts y navegación
│       ├── MainLayout.razor
│       └── NavMenu.razor
├── tests/                   # Tests unitarios e integración
│   ├── components/
│   └── integration/
├── specs/                   # Especificaciones de features
│   ├── 001-budget-preview/
│   ├── 001-currency-conversion/
│   └── ...
├── wwwroot/                 # Archivos estáticos
│   ├── css/
│   └── lib/
├── Program.cs               # Punto de entrada
└── BudgetApp.csproj        # Archivo de proyecto
```

## 🏗️ Arquitectura

### Modelos de Datos

- **Budget**: Representa un presupuesto completo con cliente, items y metadatos
- **BudgetItem**: Item individual del presupuesto con descripción, cantidad y precio
- **Client**: Información del cliente (nombre, empresa, email)
- **Currency**: Enum para diferentes monedas soportadas

### Servicios

- **BudgetService**: Gestión de presupuestos (crear, guardar, cargar, eliminar)
- **CalculationService**: Cálculos de subtotales y totales
- **CurrencyService**: Conversión de monedas y gestión de tasas de cambio
- **LocalStorageService**: Abstracción para operaciones de LocalStorage
- **AutoSaveService**: Guardado automático de cambios

### Componentes Principales

- **BudgetPreview**: Vista previa profesional del presupuesto
- **BudgetEditor**: Editor completo de presupuestos
- **BudgetList**: Lista de presupuestos guardados
- **ClientInfo**: Visualización y edición de información del cliente

## 🎨 Tecnologías Utilizadas

- **Blazor WebAssembly 10.0**: Framework para aplicaciones web interactivas
- **Bootstrap**: Framework CSS para el diseño responsive
- **Blazored.LocalStorage**: Biblioteca para acceso a LocalStorage
- **bunit**: Framework de testing para componentes Blazor
- **xUnit**: Framework de testing unitario

## 📝 Funcionalidades Detalladas

### Gestión de Presupuestos

- Crear nuevos presupuestos con información del cliente
- Agregar, editar y eliminar items del presupuesto
- Cálculo automático de subtotales (cantidad × precio unitario)
- Cálculo automático del total final
- Formato monetario con separadores de miles y 2 decimales

### Vista Previa

- Visualización profesional del presupuesto completo
- Muestra información del cliente de forma destacada
- Tabla organizada con todos los items
- Cálculos intermedios y total final claramente visibles
- Formato limpio y legible

### Persistencia

- Guardado automático en LocalStorage del navegador
- Carga de presupuestos guardados al iniciar la aplicación
- Lista de presupuestos guardados para selección
- Manejo de errores cuando LocalStorage está lleno o no disponible

### Conversión de Monedas

- Soporte para múltiples monedas (Pesos, Dólares, Euros, etc.)
- Configuración de tasas de cambio
- Conversión automática de valores

## 🔧 Configuración

### LocalStorage

Los presupuestos se guardan automáticamente en LocalStorage del navegador. La clave utilizada es `"budgets"`.

### Monedas

Las monedas disponibles se definen en el enum `Currency`. Las tasas de cambio se configuran a través del componente `ExchangeRateConfig`.

## 📚 Especificaciones

El proyecto incluye especificaciones detalladas en la carpeta `specs/`:

- `001-budget-preview/`: Vista previa y persistencia de presupuestos
- `001-currency-conversion/`: Conversión de monedas
- `001-create-budget/`: Creación de presupuestos
- `001-capture-client-info/`: Captura de información del cliente

## 🤝 Contribuir

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto es parte de SpecKit Template.

## 👥 Autor

Desarrollado como parte del template SpecKit para Cursor Agent.

---

**Nota**: Esta aplicación utiliza LocalStorage del navegador para persistencia. Los datos se almacenan localmente y no se sincronizan entre diferentes navegadores o dispositivos.


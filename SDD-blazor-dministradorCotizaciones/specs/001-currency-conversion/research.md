# Research: Conversión de Moneda y Cálculo de Presupuesto

**Feature**: 001-currency-conversion  
**Date**: 2025-01-27  
**Purpose**: Resolver decisiones técnicas para implementación de conversión de moneda

## Research Questions & Decisions

### Q1: Currency Storage Format

**Question**: ¿Cómo almacenar la moneda seleccionada en el modelo Budget?

**Alternatives Considered**:
1. **String**: `"PESOS"` o `"DOLLARS"` - Simple pero propenso a errores de tipeo
2. **Enum**: `Currency.Pesos` o `Currency.Dollars` - Type-safe, compilación segura
3. **Record separado**: `CurrencyInfo` con código y símbolo - Más flexible pero más complejo

**Decision**: **Enum `Currency`** con valores `Pesos` y `Dollars`

**Rationale**:
- Type-safety en C# 13
- Fácil de validar y comparar
- Serialización JSON automática a string
- Extensible para futuras monedas
- Compatible con Data Annotations para validación

**Implementation**:
```csharp
public enum Currency
{
    Pesos,
    Dollars
}
```

---

### Q2: Exchange Rate Storage

**Question**: ¿Dónde y cómo persistir la tasa de cambio?

**Alternatives Considered**:
1. **Global en LocalStorage**: Una tasa única para toda la aplicación
2. **Por presupuesto**: Cada presupuesto guarda su tasa de cambio
3. **En sesión (servicio)**: Solo en memoria durante la sesión actual
4. **Híbrido**: Sesión con opción de guardar por presupuesto

**Decision**: **Híbrido - Sesión con opción de guardar por presupuesto**

**Rationale**:
- La tasa cambia frecuentemente, no tiene sentido forzar una tasa global
- Guardar por presupuesto permite preservar el contexto histórico
- Sesión permite trabajar sin persistir si no se desea
- Flexibilidad para el usuario

**Implementation**:
- `ICurrencyService` mantiene tasa en sesión
- Opcionalmente, `Budget` puede tener `ExchangeRate` nullable para preservar contexto histórico
- Por defecto, tasa se configura en sesión y se aplica a conversiones

---

### Q3: Base Currency Strategy

**Question**: ¿Cómo mantener valores base para reconversión precisa?

**Alternatives Considered**:
1. **Solo valores convertidos**: Guardar solo valores en moneda actual
2. **Valores base + moneda actual**: Guardar valores originales en moneda base
3. **Historial de conversiones**: Guardar todas las conversiones realizadas

**Decision**: **Valores base en Pesos + moneda de visualización**

**Rationale**:
- FR-010 requiere mantener valores base para reconversión sin pérdida de precisión
- Pesos como moneda base (asumido en spec)
- Al cambiar moneda, se calculan valores convertidos desde base
- Permite cambiar múltiples veces sin pérdida de precisión

**Implementation**:
- `BudgetItem.UnitPrice` siempre en moneda base (Pesos)
- `Budget.Currency` indica moneda de visualización
- Conversión calculada on-the-fly al mostrar/calcular
- Al cambiar moneda, se actualiza `Budget.Currency` pero no los valores base

---

### Q4: Currency Formatting

**Question**: ¿Cómo formatear valores según moneda seleccionada?

**Alternatives Considered**:
1. **Símbolo único**: `$` para ambas (confuso)
2. **Código ISO**: `ARS`, `USD` (claro pero verboso)
3. **Símbolo con contexto**: `$` con código o prefijo (`ARS $`, `USD $`)
4. **Configurable**: Permitir al usuario elegir formato

**Decision**: **Símbolo con código ISO como prefijo** (`ARS $1,234.56` o `USD $1,234.56`)

**Rationale**:
- Claridad: diferencia entre pesos y dólares
- Estándar: códigos ISO son reconocidos internacionalmente
- Consistente: mismo formato en toda la aplicación
- Extensible: fácil agregar más monedas

**Implementation**:
- Extender `ICalculationService.FormatCurrency(decimal amount, Currency currency)`
- Usar `CultureInfo` para formato numérico
- Agregar prefijo de código ISO según moneda

---

### Q5: Conversion Precision

**Question**: ¿Cómo manejar precisión en conversiones bidireccionales?

**Alternatives Considered**:
1. **Redondeo directo**: Convertir y redondear inmediatamente
2. **Precisión extendida**: Mantener más decimales internamente, redondear al mostrar
3. **Redondeo bancario**: `MidpointRounding.ToEven` (estándar financiero)

**Decision**: **Redondeo bancario a 2 decimales en todas las conversiones**

**Rationale**:
- Estándar financiero (IEEE 754)
- Ya usado en `CalculationService` existente
- 2 decimales es estándar para valores monetarios
- Consistente con el resto del sistema

**Implementation**:
- Usar `Math.Round(value, 2, MidpointRounding.ToEven)` en conversiones
- Aplicar redondeo al convertir, no solo al mostrar
- Validar que resultados estén en rango válido

---

### Q6: Exchange Rate Validation

**Question**: ¿Qué validaciones aplicar a la tasa de cambio?

**Alternatives Considered**:
1. **Solo > 0**: Validación mínima
2. **Rango razonable**: 0.001 a 10,000 (prevenir errores de entrada)
3. **Sin límites**: Confiar en el usuario

**Decision**: **Validación de rango razonable (0.001 a 10,000)**

**Rationale**:
- Previene errores comunes (ceros, negativos, valores extremos)
- Permite tasas realistas (ej: 1 USD = 0.001 pesos o 1 peso = 10,000 USD)
- Balance entre seguridad y flexibilidad
- Mensajes de error claros

**Implementation**:
- Data Annotation: `[Range(0.001, 10000, ErrorMessage = "...")]`
- Validación en servicio antes de aplicar conversión
- Mensaje de error descriptivo

---

## Integration Points

### Existing Services

- **ICalculationService**: Extender con métodos de formateo por moneda
- **IBudgetService**: No requiere cambios (moneda se persiste con budget)
- **ILocalStorageService**: No requiere cambios (serialización JSON maneja enum)

### New Services

- **ICurrencyService**: Nuevo servicio para:
  - Configurar tasa de cambio
  - Convertir valores entre monedas
  - Obtener moneda actual del presupuesto
  - Validar tasa de cambio

### Components

- **CurrencySelector.razor**: Dropdown/radio buttons para seleccionar moneda
- **ExchangeRateConfig.razor**: Formulario para configurar tasa de cambio
- **Existing components**: Actualizar para usar formateo por moneda

---

## Technical Constraints

1. **.NET 10 only**: No usar bibliotecas externas para conversión de moneda
2. **Blazor WASM**: Toda la lógica debe ejecutarse en cliente
3. **LocalStorage**: Persistencia limitada a LocalStorage del navegador
4. **No backend**: No hay API para obtener tasas de cambio en tiempo real
5. **Offline-first**: Debe funcionar sin conexión a internet

---

## Open Questions Resolved

✅ **Q1**: Enum `Currency` para type-safety  
✅ **Q2**: Tasa de cambio en sesión con opción de guardar por presupuesto  
✅ **Q3**: Valores base en Pesos, conversión on-the-fly  
✅ **Q4**: Formato con código ISO como prefijo  
✅ **Q5**: Redondeo bancario a 2 decimales  
✅ **Q6**: Validación de rango razonable (0.001 a 10,000)

**Status**: ✅ **All research questions resolved** - Ready for Phase 1 design


using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Models;

/// <summary>
/// Represents a line item in a budget with description, quantity, unit price, and calculated subtotal.
/// </summary>
public record BudgetItem
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required(ErrorMessage = "La descripción del item es requerida")]
    [StringLength(500, MinimumLength = 1, ErrorMessage = "La descripción debe tener entre 1 y 500 caracteres")]
    public string Description { get; init; } = string.Empty;

    [Required(ErrorMessage = "La cantidad es requerida")]
    [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
    public decimal Quantity { get; init; }

    [Required(ErrorMessage = "El precio unitario es requerido")]
    [Range(0, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor o igual a cero")]
    public decimal UnitPrice { get; init; }

    /// <summary>
    /// Calculated subtotal (Quantity × UnitPrice). Read-only property.
    /// </summary>
    public decimal Subtotal => Math.Round(Quantity * UnitPrice, 2, MidpointRounding.ToEven);
}


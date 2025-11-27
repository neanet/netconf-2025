using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Models;

/// <summary>
/// Represents client/customer information associated with a budget.
/// </summary>
public record Client
{
    [Required(ErrorMessage = "El nombre del cliente es requerido")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 200 caracteres")]
    public string Name { get; init; } = string.Empty;

    [StringLength(200, ErrorMessage = "El nombre de la empresa no puede exceder 200 caracteres")]
    public string? Company { get; init; }

    [Required(ErrorMessage = "El email del cliente es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string Email { get; init; } = string.Empty;
}


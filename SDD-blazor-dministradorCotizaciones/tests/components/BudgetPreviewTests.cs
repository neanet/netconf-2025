using Bunit;
using BudgetApp.Components;
using BudgetApp.Models;
using BudgetApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BudgetApp.Tests.Components;

/// <summary>
/// Component tests for BudgetPreview component.
/// Tests verify that subtotals and total are displayed with proper formatting.
/// </summary>
public class BudgetPreviewTests : TestContext
{
    private void SetupServices()
    {
        Services.AddScoped<ICalculationService, CalculationService>();
    }

    [Fact]
    public void BudgetPreview_ShouldDisplayFormattedSubtotals()
    {
        // Arrange
        SetupServices();
        var budget = new Budget
        {
            Id = Guid.NewGuid(),
            Client = new Client
            {
                Name = "Test Client",
                Email = "test@example.com"
            },
            Items = new List<BudgetItem>
            {
                new BudgetItem
                {
                    Description = "Item 1",
                    Quantity = 5,
                    UnitPrice = 100.50m
                },
                new BudgetItem
                {
                    Description = "Item 2",
                    Quantity = 10,
                    UnitPrice = 250.75m
                }
            },
            CreatedDate = DateTime.UtcNow,
            LastModifiedDate = DateTime.UtcNow
        };

        // Act
        var cut = RenderComponent<BudgetPreview>(parameters => parameters
            .Add(p => p.Budget, budget));

        // Assert
        var markup = cut.Markup;
        Assert.Contains("502.50", markup); // 5 * 100.50 = 502.50
        Assert.Contains("2,507.50", markup); // 10 * 250.75 = 2,507.50
    }

    [Fact]
    public void BudgetPreview_ShouldDisplayFormattedTotal()
    {
        // Arrange
        SetupServices();
        var budget = new Budget
        {
            Id = Guid.NewGuid(),
            Client = new Client
            {
                Name = "Test Client",
                Email = "test@example.com"
            },
            Items = new List<BudgetItem>
            {
                new BudgetItem
                {
                    Description = "Item 1",
                    Quantity = 2,
                    UnitPrice = 1000m
                }
            },
            CreatedDate = DateTime.UtcNow,
            LastModifiedDate = DateTime.UtcNow
        };

        // Act
        var cut = RenderComponent<BudgetPreview>(parameters => parameters
            .Add(p => p.Budget, budget));

        // Assert
        var markup = cut.Markup;
        Assert.Contains("2,000.00", markup); // Total should be 2,000.00
    }

    [Fact]
    public void BudgetPreview_ShouldCalculateTotalCorrectly()
    {
        // Arrange
        SetupServices();
        var budget = new Budget
        {
            Id = Guid.NewGuid(),
            Client = new Client
            {
                Name = "Test Client",
                Email = "test@example.com"
            },
            Items = new List<BudgetItem>
            {
                new BudgetItem
                {
                    Description = "Item 1",
                    Quantity = 5,
                    UnitPrice = 100m
                },
                new BudgetItem
                {
                    Description = "Item 2",
                    Quantity = 3,
                    UnitPrice = 50m
                }
            },
            CreatedDate = DateTime.UtcNow,
            LastModifiedDate = DateTime.UtcNow
        };

        // Act
        var cut = RenderComponent<BudgetPreview>(parameters => parameters
            .Add(p => p.Budget, budget));

        // Assert
        // Total should be: (5 * 100) + (3 * 50) = 500 + 150 = 650
        var markup = cut.Markup;
        Assert.Contains("650.00", markup);
    }
}


using Bunit;
using BudgetApp.Services;
using Xunit;

namespace BudgetApp.Tests.Components;

/// <summary>
/// Component tests for CalculationService.FormatCurrency method.
/// Tests verify proper monetary formatting with 2 decimal places and thousand separators.
/// </summary>
public class CalculationServiceTests
{
    [Fact]
    public void FormatCurrency_ShouldFormatWithTwoDecimals()
    {
        // Arrange
        var service = new CalculationService();
        var amount = 1234.5m;

        // Act
        var result = service.FormatCurrency(amount);

        // Assert
        Assert.Equal("1,234.50", result);
    }

    [Fact]
    public void FormatCurrency_ShouldIncludeThousandSeparators()
    {
        // Arrange
        var service = new CalculationService();
        var amount = 1234567.89m;

        // Act
        var result = service.FormatCurrency(amount);

        // Assert
        Assert.Equal("1,234,567.89", result);
    }

    [Fact]
    public void FormatCurrency_ShouldHandleSmallAmounts()
    {
        // Arrange
        var service = new CalculationService();
        var amount = 0.01m;

        // Act
        var result = service.FormatCurrency(amount);

        // Assert
        Assert.Equal("0.01", result);
    }

    [Fact]
    public void FormatCurrency_ShouldHandleZero()
    {
        // Arrange
        var service = new CalculationService();
        var amount = 0m;

        // Act
        var result = service.FormatCurrency(amount);

        // Assert
        Assert.Equal("0.00", result);
    }

    [Fact]
    public void FormatCurrency_ShouldRoundToTwoDecimals()
    {
        // Arrange
        var service = new CalculationService();
        var amount = 1234.567m;

        // Act
        var result = service.FormatCurrency(amount);

        // Assert
        Assert.Equal("1,234.57", result); // Rounded up
    }
}


using TaxCalculator.API.Services;
using TaxCalculator.API.Models;
using FluentAssertions;

namespace TaxCalculator.Tests.Services
{
    public class TestTaxCalculatorService
    {
        private List<TaxBand> GetDefaultBands() =>
            new List<TaxBand>
            {
                new TaxBand { Min = 0, Max = 5000, Rate = 0.0 },
                new TaxBand { Min = 5000, Max = 20000, Rate = 0.2 },
                new TaxBand { Min = 20001, Max = null, Rate = 0.4 }
            };

        [Theory]
        [InlineData(0, 0)]
        [InlineData(4000, 0)]
        public void CalculateTax_ReturnsCorrectTax(decimal salary, decimal expectedTax)
        {
            var bands = GetDefaultBands();
            var calculator = new TaxCalculator.API.Services.TaxCalculator(bands);
            var tax = calculator.CalculateTax(salary);
            tax.Should().BeApproximately(expectedTax, 0.01m);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1000, 0)]  // Negative salary returns 0 tax
        public void CalculateTax_HandlesZeroAndNegativeSalary(decimal salary, decimal expectedTax)
        {
            var bands = GetDefaultBands();
            var calculator = new TaxCalculator.API.Services.TaxCalculator(bands);
            var tax = calculator.CalculateTax(salary);
            tax.Should().BeApproximately(expectedTax, 0.01m);
        }

        [Fact]
        public void CalculateTax_WithEmptyBands_ReturnsZero()
        {
            var bands = new List<TaxBand>();
            var calculator = new TaxCalculator.API.Services.TaxCalculator(bands);
            var tax = calculator.CalculateTax(10000);
            tax.Should().Be(0);
        }
    }
}

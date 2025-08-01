using TaxCalculator.API.Services;
using TaxCalculator.API.Models;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;

namespace TaxCalculator.Tests.Services
{
    public class TestTaxCalculatorService
    {
        private TaxCalculator.API.Services.TaxCalculator CreateCalculator()
        {
            var mockLogger = new Mock<ILogger<TaxCalculator.API.Services.TaxCalculator>>();
            return new TaxCalculator.API.Services.TaxCalculator(mockLogger.Object);
        }

        private List<TaxBand> GetDefaultBands() =>
            new List<TaxBand>
            {
                new TaxBand { Min = 0, Max = 5000, Rate = 0.0 },
                new TaxBand { Min = 5000, Max = 20000, Rate = 0.2 },
                new TaxBand { Min = 20001, Max = null, Rate = 0.4 }
            };

        [Theory]
        [InlineData(0)]
        [InlineData(-1000)]
        public void Calculate_HandlesZeroAndNegativeSalary(decimal salary)
        {
            var bands = GetDefaultBands();
            var calculator = CreateCalculator();

            Action act = () => calculator.CalculateTax(salary, bands);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Calculate_WithMiddleBandSalary()
        {
            var bands = GetDefaultBands();
            var calculator = CreateCalculator();

            var result = calculator.CalculateTax(10000m, bands);

            result.GrossAnnualSalary.Should().Be(10000m);
            result.GrossMonthlySalary.Should().BeApproximately(833.33m, 0.01m);
            result.AnnualTaxPaid.Should().BeApproximately(1000m, 0.01m);
            result.MonthlyTaxPaid.Should().BeApproximately(83.33m, 0.01m);
            result.NetAnnualSalary.Should().BeApproximately(9000m, 0.01m);
            result.NetMonthlySalary.Should().BeApproximately(750m, 0.01m);
        }

        [Fact]
        public void Calculate_WithLowerBandSalary()
        {
            var bands = GetDefaultBands();
            var calculator = CreateCalculator();

            var result = calculator.CalculateTax(4000m, bands);

            result.GrossAnnualSalary.Should().Be(4000m);
            result.GrossMonthlySalary.Should().BeApproximately(333.33m, 0.01m);
            result.AnnualTaxPaid.Should().BeApproximately(0, 0.01m);
            result.MonthlyTaxPaid.Should().BeApproximately(0, 0.01m);
            result.NetAnnualSalary.Should().BeApproximately(4000m, 0.01m);
            result.NetMonthlySalary.Should().BeApproximately(333.33m, 0.01m);
        }

        [Fact]
        public void Calculate_WithHigherBandSalary()
        {
            var bands = GetDefaultBands();
            var calculator = CreateCalculator();

            var result = calculator.CalculateTax(38000m, bands);

            result.GrossAnnualSalary.Should().Be(38000m);
            result.GrossMonthlySalary.Should().BeApproximately(3166.67m, 0.01m);
            result.AnnualTaxPaid.Should().BeApproximately(10200.0m, 0.01m);
            result.MonthlyTaxPaid.Should().BeApproximately(850.0m, 0.01m);
            result.NetAnnualSalary.Should().BeApproximately(27800.0m, 0.01m);
            result.NetMonthlySalary.Should().BeApproximately(2316.67m, 0.01m);
        }

        [Fact]
        public void Calculate_WithUltraHigherBandSalary()
        {
            var bands = GetDefaultBands();
            var calculator = CreateCalculator();

            var result = calculator.CalculateTax(155000m, bands);

            result.GrossAnnualSalary.Should().Be(155000m);
            result.GrossMonthlySalary.Should().BeApproximately(12916.67m, 0.01m);
            result.AnnualTaxPaid.Should().BeApproximately(57000.0m, 0.01m);
            result.MonthlyTaxPaid.Should().BeApproximately(4750.0m, 0.01m);
            result.NetAnnualSalary.Should().BeApproximately(98000.0m, 0.01m);
            result.NetMonthlySalary.Should().BeApproximately(8166.67m, 0.01m);
        }
    }
}

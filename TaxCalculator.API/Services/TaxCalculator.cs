using Microsoft.Extensions.Logging;
using TaxCalculator.API.Data;
using TaxCalculator.API.Models;

namespace TaxCalculator.API.Services;

public class TaxCalculator : ITaxCalculator
{
    private readonly ILogger<TaxCalculator> _logger;

    public TaxCalculator(ILogger<TaxCalculator> logger)
    {
        _logger = logger;
    }

    public TaxResult CalculateTax(decimal income, List<TaxBand> taxBands)
    {
        if (income <= 0)
            throw new ArgumentOutOfRangeException(nameof(income), "Income must be greater than zero.");

        if (taxBands == null || !taxBands.Any())
        {
            _logger.LogWarning("No tax bands provided for income calculation.");
            return CreateZeroTaxResult(income);
        }

        taxBands = taxBands.OrderBy(b => b.Min).ToList();

        decimal totalTax = 0m;
        decimal incomeLeft = income;

        TaxResult taxResult = new TaxResult
        {
            GrossAnnualSalary = Math.Round(income, 2),
            GrossMonthlySalary = Math.Round(income / 12m, 2)
        };

        foreach (var band in taxBands)
        {
            decimal bandMin = (decimal)band.Min;
            decimal bandMax = band.Max.HasValue ? (decimal)band.Max.Value : decimal.MaxValue;
            decimal bandRange = bandMax - bandMin;

            if (income > bandMin)
            {
                decimal taxableAmount = Math.Min(incomeLeft, bandRange);
                decimal bandTax = taxableAmount * (decimal)band.Rate;

                totalTax += bandTax;
                incomeLeft -= taxableAmount;

                if (incomeLeft <= 0)
                    break;
            }
        }

        taxResult.AnnualTaxPaid = Math.Round(totalTax, 2);
        taxResult.MonthlyTaxPaid = Math.Round(totalTax / 12m, 2);
        taxResult.NetAnnualSalary = Math.Round(income - totalTax, 2);
        taxResult.NetMonthlySalary = Math.Round(taxResult.NetAnnualSalary / 12m, 2);

        return taxResult;
    }

    private TaxResult CreateZeroTaxResult(decimal income)
    {
        return new TaxResult
        {
            GrossAnnualSalary = Math.Round(income, 2),
            GrossMonthlySalary = Math.Round(income / 12m, 2),
            NetAnnualSalary = Math.Round(income, 2),
            NetMonthlySalary = Math.Round(income / 12m, 2),
            AnnualTaxPaid = 0m,
            MonthlyTaxPaid = 0m
        };
    }
}

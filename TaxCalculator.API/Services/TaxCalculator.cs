using Microsoft.EntityFrameworkCore;
using TaxCalculator.API.Data;
using TaxCalculator.API.Models;

namespace TaxCalculator.API.Services;

public class TaxCalculator : ITaxCalculator
{
    private readonly IEnumerable<TaxBand> _taxBands;

    public TaxCalculator(IEnumerable<TaxBand> bands)
    {
        _taxBands = bands.OrderBy(b => b.Min);
    }

    public TaxResult CalculateTax(decimal income) => new TaxResult
    {
        GrossAnnualSalary = income,
        GrossMonthlySalary = income / 12,
        NetAnnualSalary = income,
        NetMonthlySalary = income / 12,
        AnnualTaxPaid = 0,
        MonthlyTaxPaid = 0
    };

    public TaxResult CalculateTax(decimal income, List<TaxBand> taxBands)
    {
        if (income <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(income), "*Income cannot be negative or zero*");
        }

        decimal totalTax = 0;
        decimal incomeLeft = income;

        TaxResult taxResult = new TaxResult
        {
            GrossAnnualSalary = Math.Round(income, 2),
            GrossMonthlySalary = Math.Round(income / 12, 2)
        };

        foreach (var band in taxBands)
        {
            if (incomeLeft <= 0)
                break;

            decimal bandMin = (decimal)band.Min;
            decimal bandMax = (decimal?)(band.Max) ?? decimal.MaxValue;
            decimal taxableAmount = Math.Min(incomeLeft, bandMax - bandMin);

            if (taxableAmount > 0)
            {
                totalTax += taxableAmount * ((decimal)band.Rate);
                incomeLeft -= taxableAmount;
            }
        }

        taxResult.NetAnnualSalary = Math.Round(income - totalTax, 2);
        taxResult.NetMonthlySalary = Math.Round(taxResult.NetAnnualSalary / 12, 2);
        taxResult.AnnualTaxPaid = Math.Round(totalTax, 2);
        taxResult.MonthlyTaxPaid = Math.Round(totalTax / 12, 2);

        return taxResult;
    }
}
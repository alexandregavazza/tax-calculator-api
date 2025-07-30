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

    public decimal CalculateTax(decimal income) => 0;

    public decimal CalculateTax(decimal income, List<TaxBand> taxBands)
    {
        decimal totalTax = 0;
        decimal incomeLeft = income;

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

        return totalTax;
    }
}
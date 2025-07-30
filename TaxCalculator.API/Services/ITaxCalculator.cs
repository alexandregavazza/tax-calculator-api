using TaxCalculator.API.Models;

namespace TaxCalculator.API.Services;

public interface ITaxCalculator
{
    TaxResult CalculateTax(decimal income);
    TaxResult CalculateTax(decimal income, List<TaxBand> taxBands);
}
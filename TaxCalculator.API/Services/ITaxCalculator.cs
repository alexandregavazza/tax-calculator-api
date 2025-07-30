using TaxCalculator.API.Models;

namespace TaxCalculator.API.Services;

public interface ITaxCalculator
{
    decimal CalculateTax(decimal income);
    decimal CalculateTax(decimal income, List<TaxBand> taxBands);
}
using TaxCalculator.API.Models;

namespace TaxCalculator.API.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.TaxBands.Any()) return;

            var bands = new List<TaxBand>
            {
                new TaxBand { Min = 0, Max = 5000, Rate = 0.00 },
                new TaxBand { Min = 5001, Max = 20000, Rate = 0.20 },
                new TaxBand { Min = 20001, Max = null, Rate = 0.40 }
            };

            context.TaxBands.AddRange(bands);
            context.SaveChanges();
        }
    }
}
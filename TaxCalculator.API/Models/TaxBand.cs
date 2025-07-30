namespace TaxCalculator.API.Models;
public class TaxBand
{
        public int Id { get; set; }

        public double Min { get; set; }

        public double? Max { get; set; } // null means no upper limit

        public double Rate { get; set; } // e.g., 0.15 for 15%
}
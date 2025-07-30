namespace TaxCalculator.API.Models
{
    public class TaxResult
    {
        public decimal GrossAnnualSalary{ get; set; }

        public decimal GrossMonthlySalary { get; set; }
        public decimal NetAnnualSalary { get; set; }
        public decimal NetMonthlySalary { get; set; }
        public decimal AnnualTaxPaid { get; set; }
        public decimal MonthlyTaxPaid { get; set; }
    }
}

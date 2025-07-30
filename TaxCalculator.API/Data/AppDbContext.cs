using Microsoft.EntityFrameworkCore;
using TaxCalculator.API.Models;

namespace TaxCalculator.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TaxBand> TaxBands => Set<TaxBand>();
    }
}
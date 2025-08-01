using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TaxCalculator.API.Data;
using TaxCalculator.API.Services;

namespace TaxCalculatorAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaxController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ITaxCalculator _calculator;
    private readonly IDistributedCache _cache;
    private readonly ILogger<TaxController> _logger;

    public TaxController(AppDbContext context, ITaxCalculator calculator, IDistributedCache cache, ILogger<TaxController> logger)
    {
        _context = context;
        _calculator = calculator;
        _cache = cache;
        _logger = logger;
    }

    [HttpGet("{income:decimal}")]
    public async Task<IActionResult> GetTax(decimal income)
    {
        if (income < 0)
        {
            return BadRequest("Income must be a non-negative value.");
        }

        string cacheKey = $"tax:{income}";

        try
        {
            var cached = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cached))
            {
                var cachedTax = JsonSerializer.Deserialize<object>(cached);
                return Ok(cachedTax);
            }

            var bands = await _context.TaxBands.ToListAsync();

            if (!bands.Any())
            {
                return StatusCode(500, "No tax bands configured in the database.");
            }

            var tax = _calculator.CalculateTax(income, bands);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            var serializedTax = JsonSerializer.Serialize(tax);
            await _cache.SetStringAsync(cacheKey, serializedTax, options);

            return Ok(tax);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while calculating tax.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}

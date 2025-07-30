using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
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

    public TaxController(AppDbContext context, ITaxCalculator calculator, IDistributedCache cache)
    {
        _context = context;
        _calculator = calculator;
        _cache = cache;
    }

    [HttpGet("{income}")]
    public async Task<IActionResult> GetTax(decimal income)
    {
        string cacheKey = $"tax:{income}";
        string cachedTax = await _cache.GetStringAsync(cacheKey);

        if (cachedTax != null)
        {
            return Ok(JsonSerializer.Serialize(cachedTax));
        }

        var bands = _context.TaxBands.ToList();
        var tax = _calculator.CalculateTax(income, bands);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };

        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(tax), options);
        return Ok(JsonSerializer.Serialize(tax));
    }
}
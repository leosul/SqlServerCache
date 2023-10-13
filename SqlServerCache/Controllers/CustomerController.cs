using Microsoft.AspNetCore.Mvc;
using SqlServerCache.Services;

namespace SqlServerCache.Controllers;

[ApiController]
[Route("api/v1/customers")]
public class CustomerController : ControllerBase
{
    [HttpGet("from-cache")]
    public async Task<IActionResult> GetCustomersFromCacheAsync()
    {
        var customers = await new CacheService().GetCustomersFromCacheAsync();

        return Ok(customers.Take(100));
    }

    [HttpGet("from-database")]
    public async Task<IActionResult> GetCustomersFromDatabaseAsync()
    {
        var customers = await new CacheService().GetCustomersFromDatabaseAsync();

        return Ok(customers.Take(100));
    }

    [HttpGet("set-cache")]
    public async Task<IActionResult> SetCacheAsync()
    {
        var customers = await new CacheService().FetchCustomersFromDatabase();

        return Ok(customers.Take(100));
    }
}

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SqlServerCache.Data.Context;
using SqlServerCache.Models;

namespace SqlServerCache.Services;

public class CacheService
{
    private readonly IDistributedCache _cache;

    public CacheService()
    {
        var serviceProvider = new ServiceCollection()
            .AddDistributedSqlServerCache(o =>
            {
                o.ConnectionString = "Server=.\\SQLEXPRESS;Database=sqlcache;Trusted_Connection=True;TrustServerCertificate=True;";
                o.SchemaName = "dbo";
                o.TableName = "CustomerCache";
            })
            .BuildServiceProvider();

        _cache = serviceProvider.GetRequiredService<IDistributedCache>();
    }
    public async Task<IEnumerable<Customer>> GetCustomersFromCacheAsync()
    {
        var cached = await _cache.GetStringAsync("Customers");
        
        return JsonSerializer.Deserialize<List<Customer>>(cached);
    }

    public async Task<List<Customer>> GetCustomersFromDatabaseAsync()
    {
        using var context = new SqlCacheDbContext();

        return await context.Customers.ToListAsync();
    }

    public async Task<List<Customer>> FetchCustomersFromDatabase()
    {
        var customers = new List<Customer>();
        using (var context = new SqlCacheDbContext())
        {
            var dataToRemove = await context.Customers.ToListAsync();
            if(dataToRemove.Any()) context.RemoveRange(dataToRemove);

            for (int i = 0; i < 2000; i++)
            {
                customers.Add(new Customer
                {
                    Name = Guid.NewGuid().ToString(),
                    IsActive = i % 2 == 0,
                });
            }

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }

        var cacheOptions = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            SlidingExpiration = TimeSpan.FromMinutes(10),
        };

        var customerJson = JsonSerializer.Serialize(customers);
        await _cache.SetStringAsync("Customers", customerJson, cacheOptions);

        return customers;
    }
}

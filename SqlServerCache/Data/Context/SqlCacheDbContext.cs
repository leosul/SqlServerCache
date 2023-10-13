using Microsoft.EntityFrameworkCore;
using SqlServerCache.Models;

namespace SqlServerCache.Data.Context;

public class SqlCacheDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=sqlcache;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerCache> CustomerCache { get; set; }
}

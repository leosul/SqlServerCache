using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SqlServerCache.Services;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<ValidateCache>();
    }

    [MemoryDiagnoser]
    public class ValidateCache
    {
        [Benchmark]
        public async Task GetWithSqlCacheData()
        {
            var customers = await new CacheService().GetCustomersFromCacheAsync();
        }

        [Benchmark]
        public async Task GetWithOutSqlCacheData()
        {
            var customers = await new CacheService().GetCustomersFromDatabaseAsync();
        }
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SqlServerCache.Models;

public class CustomerCache
{
    [Key]
    public string Id { get; set; }
    public byte[] Value { get; set; }
    public DateTimeOffset ExpiresAtTime { get; set; }
    public int SlidingExpirationInSeconds { get; set; }
    public DateTimeOffset AbsoluteExpiration { get; set; }
}

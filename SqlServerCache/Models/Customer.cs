using System.ComponentModel.DataAnnotations;

namespace SqlServerCache.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}

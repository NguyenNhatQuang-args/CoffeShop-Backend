using System;

namespace WebApp_Coffe.Models;

public class ProductVariant
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public string SizeName { get; set; } = string.Empty; // S/M/L
    public string Temperature { get; set; } = string.Empty; // Hot/Cold/Both
    
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;
}

using System;

namespace WebApp_Coffe.Models;

public class ProductTag
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public string TagName { get; set; } = string.Empty; // bestseller/new/seasonal
}

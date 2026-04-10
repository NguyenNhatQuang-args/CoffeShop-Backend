using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace WebApp_Coffe.DTOs;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public List<string> Tags { get; set; } = new();
}

public class ProductDetailResponse : ProductResponse
{
    public List<ProductVariantDto> Variants { get; set; } = new();
}

public class ProductVariantDto
{
    public Guid? Id { get; set; }
    public string SizeName { get; set; } = string.Empty;
    public string Temperature { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;
}

public class ProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public IFormFile? ImageFile { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; } = false;
    public Guid CategoryId { get; set; }

    public List<ProductVariantDto> Variants { get; set; } = new();
    public List<string> Tags { get; set; } = new();
}

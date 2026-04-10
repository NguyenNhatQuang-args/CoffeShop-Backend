using System;
using System.Collections.Generic;

namespace WebApp_Coffe.DTOs;

public class CategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
}

public class CategoryDetailResponse : CategoryResponse
{
    public List<ProductResponse> Products { get; set; } = new();
}

public class CategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public IFormFile? ImageFile { get; set; }
    public bool IsActive { get; set; } = true;
}

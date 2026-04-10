using System;
using System.Collections.Generic;
using WebApp_Coffe.Models;

namespace CoffeeShop.Tests.Helpers;

public static class MockData
{
    public static List<Category> GetCategories()
    {
        var categoryId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        return new List<Category>
        {
            new Category { Id = categoryId, Name = "Coffee", Slug = "coffee", IsActive = true, Products = new List<Product>() },
            new Category { Id = Guid.NewGuid(), Name = "Tea", Slug = "tea", IsActive = false, Products = new List<Product>() }
        };
    }

    public static List<Product> GetProducts()
    {
        var categoryId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        return new List<Product>
        {
            new Product { 
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), 
                Name = "Espresso", 
                Slug = "espresso", 
                IsActive = true, 
                IsFeatured = true,
                CategoryId = categoryId,
                Category = new Category { Id = categoryId, Name = "Coffee" },
                ProductTags = new List<ProductTag> { new ProductTag { TagName = "bestseller" } },
                ProductVariants = new List<ProductVariant>()
            },
            new Product { 
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), 
                Name = "Latte", 
                Slug = "latte", 
                IsActive = true, 
                IsFeatured = false,
                CategoryId = categoryId,
                Category = new Category { Id = categoryId, Name = "Coffee" },
                ProductTags = new List<ProductTag>(),
                ProductVariants = new List<ProductVariant>()
            },
            new Product { 
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), 
                Name = "Mocha", 
                Slug = "mocha", 
                IsActive = false, 
                IsFeatured = false,
                CategoryId = categoryId,
                Category = new Category { Id = categoryId, Name = "Coffee" },
                ProductTags = new List<ProductTag>(),
                ProductVariants = new List<ProductVariant>()
            }
        };
    }
}

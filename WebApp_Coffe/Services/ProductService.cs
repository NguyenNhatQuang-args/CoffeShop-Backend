using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp_Coffe.DTOs;
using WebApp_Coffe.DTOs.Common;
using WebApp_Coffe.Models;
using WebApp_Coffe.Repositories;

namespace WebApp_Coffe.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    private readonly IFileUploadService _uploadService;

    public ProductService(IProductRepository repo, IFileUploadService uploadService)
    {
        _repo = repo;
        _uploadService = uploadService;
    }

    public async Task<ApiResponse<PagedResult<ProductResponse>>> GetProductsAsync(Guid? categoryId, string? tag, string? search, bool? isFeature, int page, int pageSize)
    {
        var query = _repo.Query().Include(p => p.Category).Include(p => p.ProductTags).Include(p => p.ProductVariants).Where(p => p.IsActive);

        if (categoryId.HasValue) query = query.Where(p => p.CategoryId == categoryId.Value);
        if (isFeature.HasValue) query = query.Where(p => p.IsFeatured == isFeature.Value);
        if (!string.IsNullOrEmpty(search)) query = query.Where(p => p.Name.Contains(search) || (p.Description != null && p.Description.Contains(search)));
        if (!string.IsNullOrEmpty(tag)) query = query.Where(p => p.ProductTags.Any(t => t.TagName == tag));

        int totalCount = await query.CountAsync();
        var items = await query.OrderByDescending(p => p.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var data = items.Select(p => new ProductResponse { Id = p.Id, Name = p.Name, Slug = p.Slug, Description = p.Description, ImageUrl = p.ImageUrl, IsActive = p.IsActive, IsFeatured = p.IsFeatured, CategoryId = p.CategoryId, CategoryName = p.Category.Name, MinPrice = p.ProductVariants.Any() ? p.ProductVariants.Min(v => v.Price) : null, MaxPrice = p.ProductVariants.Any() ? p.ProductVariants.Max(v => v.Price) : null, Tags = p.ProductTags.Select(t => t.TagName).ToList() }).ToList();
        
        return ApiResponse<PagedResult<ProductResponse>>.Ok(new PagedResult<ProductResponse> { Items = data, TotalCount = totalCount, PageNumber = page, PageSize = pageSize });
    }

    public async Task<ApiResponse<ProductDetailResponse>> GetBySlugAsync(string slug)
    {
        var product = await _repo.GetBySlugAsync(slug);
        if (product == null || !product.IsActive) return ApiResponse<ProductDetailResponse>.Fail("Product not found");

        var data = new ProductDetailResponse
        {
            Id = product.Id, Name = product.Name, Slug = product.Slug, Description = product.Description, ImageUrl = product.ImageUrl, IsActive = product.IsActive, IsFeatured = product.IsFeatured, CategoryId = product.CategoryId, CategoryName = product.Category.Name,
            Variants = product.ProductVariants.Select(v => new ProductVariantDto { Id = v.Id, SizeName = v.SizeName, Temperature = v.Temperature, Price = v.Price, IsAvailable = v.IsAvailable }).ToList(),
            Tags = product.ProductTags.Select(t => t.TagName).ToList()
        };
        return ApiResponse<ProductDetailResponse>.Ok(data);
    }

    public async Task<ApiResponse<List<ProductResponse>>> GetFeaturedAsync(int count)
    {
        var products = await _repo.GetFeaturedAsync(count);
        var data = products.Select(p => new ProductResponse { Id = p.Id, Name = p.Name, Slug = p.Slug, Description = p.Description, ImageUrl = p.ImageUrl, IsActive = p.IsActive, IsFeatured = p.IsFeatured, CategoryId = p.CategoryId, CategoryName = p.Category.Name, MinPrice = p.ProductVariants.Any() ? p.ProductVariants.Min(v => v.Price) : null, MaxPrice = p.ProductVariants.Any() ? p.ProductVariants.Max(v => v.Price) : null, Tags = p.ProductTags.Select(t => t.TagName).ToList() }).ToList();
        return ApiResponse<List<ProductResponse>>.Ok(data);
    }

    public async Task<ApiResponse<ProductResponse>> CreateAsync(ProductRequest request)
    {
        string? imageUrl = request.ImageUrl;
        if (request.ImageFile != null)
        {
            imageUrl = await _uploadService.UploadAsync(request.ImageFile, "products");
        }

        var entity = new Product
        {
            Name = request.Name, Slug = request.Name.ToSlug(), Description = request.Description, ImageUrl = imageUrl, IsActive = request.IsActive, IsFeatured = request.IsFeatured, CategoryId = request.CategoryId,
            ProductVariants = request.Variants.Select(v => new ProductVariant { SizeName = v.SizeName, Temperature = v.Temperature, Price = v.Price, IsAvailable = v.IsAvailable }).ToList(),
            ProductTags = request.Tags.Select(t => new ProductTag { TagName = t }).ToList()
        };
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<ProductResponse>.Ok(new ProductResponse { Id = entity.Id, Name = entity.Name, Slug = entity.Slug, CategoryId = entity.CategoryId });
    }

    public async Task<ApiResponse<ProductResponse>> UpdateAsync(Guid id, ProductRequest request)
    {
        var entity = await _repo.Query().Include(p => p.ProductVariants).Include(p => p.ProductTags).FirstOrDefaultAsync(p => p.Id == id);
        if (entity == null) return ApiResponse<ProductResponse>.Fail("Product not found");

        if (request.ImageFile != null)
        {
            entity.ImageUrl = await _uploadService.UploadAsync(request.ImageFile, "products");
        }
        else if (request.ImageUrl != null)
        {
            entity.ImageUrl = request.ImageUrl;
        }

        entity.Name = request.Name; entity.Slug = request.Name.ToSlug(); entity.Description = request.Description; entity.IsActive = request.IsActive; entity.IsFeatured = request.IsFeatured; entity.CategoryId = request.CategoryId; entity.UpdatedAt = DateTime.UtcNow;

        entity.ProductVariants.Clear();
        foreach(var v in request.Variants) entity.ProductVariants.Add(new ProductVariant { SizeName = v.SizeName, Temperature = v.Temperature, Price = v.Price, IsAvailable = v.IsAvailable });
        
        entity.ProductTags.Clear();
        foreach(var t in request.Tags) entity.ProductTags.Add(new ProductTag { TagName = t });

        await _repo.UpdateAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<ProductResponse>.Ok(new ProductResponse { Id = entity.Id, Name = entity.Name, Slug = entity.Slug, CategoryId = entity.CategoryId });
    }

    public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return ApiResponse<bool>.Fail("Product not found");
        await _repo.DeleteAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true);
    }
}

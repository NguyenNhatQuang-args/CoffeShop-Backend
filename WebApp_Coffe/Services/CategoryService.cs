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

public static class StringExtensions
{
    public static string ToSlug(this string value)
    {
        return value.ToLowerInvariant().Replace(" ", "-");
    }
}

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;
    private readonly IFileUploadService _uploadService;

    public CategoryService(ICategoryRepository repo, IFileUploadService uploadService)
    {
        _repo = repo;
        _uploadService = uploadService;
    }

    public async Task<ApiResponse<List<CategoryResponse>>> GetAllActiveAsync()
    {
        var categories = await _repo.Query().Where(c => c.IsActive).ToListAsync();
        var data = categories.Select(c => new CategoryResponse { Id = c.Id, Name = c.Name, Slug = c.Slug, Description = c.Description, ImageUrl = c.ImageUrl, IsActive = c.IsActive }).ToList();
        return ApiResponse<List<CategoryResponse>>.Ok(data);
    }

    public async Task<ApiResponse<CategoryDetailResponse>> GetBySlugAsync(string slug)
    {
        var category = await _repo.GetBySlugAsync(slug);
        if (category == null || !category.IsActive) return ApiResponse<CategoryDetailResponse>.Fail("Category not found");

        var data = new CategoryDetailResponse
        {
            Id = category.Id, Name = category.Name, Slug = category.Slug, Description = category.Description, ImageUrl = category.ImageUrl, IsActive = category.IsActive,
            Products = category.Products.Select(p => new ProductResponse { Id = p.Id, Name = p.Name, Slug = p.Slug, Description = p.Description, ImageUrl = p.ImageUrl, IsActive = p.IsActive, IsFeatured = p.IsFeatured, CategoryId = p.CategoryId }).ToList()
        };
        return ApiResponse<CategoryDetailResponse>.Ok(data);
    }

    public async Task<ApiResponse<CategoryResponse>> CreateAsync(CategoryRequest request)
    {
        string? imageUrl = request.ImageUrl;
        if (request.ImageFile != null)
        {
            imageUrl = await _uploadService.UploadAsync(request.ImageFile, "categories");
        }

        var entity = new Category
        {
            Name = request.Name,
            Slug = request.Name.ToSlug(),
            Description = request.Description,
            ImageUrl = imageUrl,
            IsActive = request.IsActive
        };
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<CategoryResponse>.Ok(new CategoryResponse { Id = entity.Id, Name = entity.Name, Slug = entity.Slug, Description = entity.Description, ImageUrl = entity.ImageUrl, IsActive = entity.IsActive });
    }

    public async Task<ApiResponse<CategoryResponse>> UpdateAsync(Guid id, CategoryRequest request)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return ApiResponse<CategoryResponse>.Fail("Category not found");

        if (request.ImageFile != null)
        {
            entity.ImageUrl = await _uploadService.UploadAsync(request.ImageFile, "categories");
        }
        else if (request.ImageUrl != null)
        {
            entity.ImageUrl = request.ImageUrl;
        }

        entity.Name = request.Name;
        entity.Slug = request.Name.ToSlug();
        entity.Description = request.Description;
        entity.IsActive = request.IsActive;

        await _repo.UpdateAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<CategoryResponse>.Ok(new CategoryResponse { Id = entity.Id, Name = entity.Name, Slug = entity.Slug, Description = entity.Description, ImageUrl = entity.ImageUrl, IsActive = entity.IsActive });
    }

    public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return ApiResponse<bool>.Fail("Category not found");
        await _repo.DeleteAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true);
    }
}

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

public class BlogService : IBlogService
{
    private readonly IBlogRepository _repo;
    private readonly IFileUploadService _uploadService;

    public BlogService(IBlogRepository repo, IFileUploadService uploadService)
    {
        _repo = repo;
        _uploadService = uploadService;
    }

    public async Task<ApiResponse<PagedResult<BlogPostResponse>>> GetPublishedBlogsAsync(int page, int pageSize)
    {
        var query = _repo.Query().Where(b => b.IsPublished);
        int totalCount = await query.CountAsync();
        var items = await query.OrderByDescending(b => b.PublishedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var data = items.Select(b => new BlogPostResponse { Id = b.Id, Title = b.Title, Slug = b.Slug, Thumbnail = b.Thumbnail, AuthorName = b.AuthorName, IsPublished = b.IsPublished, PublishedAt = b.PublishedAt, CreatedAt = b.CreatedAt }).ToList();
        return ApiResponse<PagedResult<BlogPostResponse>>.Ok(new PagedResult<BlogPostResponse> { Items = data, TotalCount = totalCount, PageNumber = page, PageSize = pageSize });
    }

    public async Task<ApiResponse<BlogPostDetailResponse>> GetBySlugAsync(string slug)
    {
        var blog = await _repo.GetBySlugAsync(slug);
        if (blog == null || !blog.IsPublished) return ApiResponse<BlogPostDetailResponse>.Fail("Blog post not found");

        var data = new BlogPostDetailResponse { Id = blog.Id, Title = blog.Title, Slug = blog.Slug, Content = blog.Content, Thumbnail = blog.Thumbnail, AuthorName = blog.AuthorName, IsPublished = blog.IsPublished, PublishedAt = blog.PublishedAt, CreatedAt = blog.CreatedAt };
        return ApiResponse<BlogPostDetailResponse>.Ok(data);
    }

    public async Task<ApiResponse<BlogPostResponse>> CreateAsync(BlogPostRequest request)
    {
        string? thumbnailUrl = request.Thumbnail;
        if (request.ThumbnailFile != null)
        {
            thumbnailUrl = await _uploadService.UploadAsync(request.ThumbnailFile, "blogs");
        }

        var entity = new BlogPost { Title = request.Title, Slug = request.Title.ToSlug(), Content = request.Content, Thumbnail = thumbnailUrl, AuthorName = request.AuthorName, IsPublished = request.IsPublished, PublishedAt = request.IsPublished ? DateTime.UtcNow : null };
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<BlogPostResponse>.Ok(new BlogPostResponse { Id = entity.Id, Title = entity.Title, Slug = entity.Slug });
    }

    public async Task<ApiResponse<BlogPostResponse>> UpdateAsync(Guid id, BlogPostRequest request)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return ApiResponse<BlogPostResponse>.Fail("Blog post not found");

        if (request.ThumbnailFile != null)
        {
            entity.Thumbnail = await _uploadService.UploadAsync(request.ThumbnailFile, "blogs");
        }
        else if (request.Thumbnail != null)
        {
            entity.Thumbnail = request.Thumbnail;
        }

        entity.Title = request.Title; entity.Slug = request.Title.ToSlug(); entity.Content = request.Content; entity.AuthorName = request.AuthorName;
        
        if (request.IsPublished && !entity.IsPublished) entity.PublishedAt = DateTime.UtcNow;
        entity.IsPublished = request.IsPublished;

        await _repo.UpdateAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<BlogPostResponse>.Ok(new BlogPostResponse { Id = entity.Id, Title = entity.Title, Slug = entity.Slug });
    }

    public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return ApiResponse<bool>.Fail("Blog post not found");
        
        await _repo.DeleteAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true);
    }
}

public class StoreService : IStoreService
{
    private readonly IStoreRepository _repo;

    public StoreService(IStoreRepository repo)
    {
        _repo = repo;
    }

    public async Task<ApiResponse<List<StoreResponse>>> GetAllActiveAsync()
    {
        var stores = await _repo.Query().Where(s => s.IsActive).ToListAsync();
        var data = stores.Select(s => new StoreResponse { Id = s.Id, Name = s.Name, Address = s.Address, Phone = s.Phone, OpenTime = s.OpenTime.ToString(@"hh\:mm"), CloseTime = s.CloseTime.ToString(@"hh\:mm"), GoogleMapUrl = s.GoogleMapUrl }).ToList();
        return ApiResponse<List<StoreResponse>>.Ok(data);
    }

    public async Task<ApiResponse<StoreResponse>> CreateAsync(StoreRequest request)
    {
        var entity = new Store
        {
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            OpenTime = TimeSpan.Parse(request.OpenTime),
            CloseTime = TimeSpan.Parse(request.CloseTime),
            GoogleMapUrl = request.GoogleMapUrl,
            IsActive = request.IsActive
        };
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<StoreResponse>.Ok(new StoreResponse { Id = entity.Id, Name = entity.Name, Address = entity.Address, Phone = entity.Phone, OpenTime = entity.OpenTime.ToString(@"hh\:mm"), CloseTime = entity.CloseTime.ToString(@"hh\:mm"), GoogleMapUrl = entity.GoogleMapUrl });
    }

    public async Task<ApiResponse<StoreResponse>> UpdateAsync(Guid id, StoreRequest request)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return ApiResponse<StoreResponse>.Fail("Store not found");

        entity.Name = request.Name;
        entity.Address = request.Address;
        entity.Phone = request.Phone;
        entity.OpenTime = TimeSpan.Parse(request.OpenTime);
        entity.CloseTime = TimeSpan.Parse(request.CloseTime);
        entity.GoogleMapUrl = request.GoogleMapUrl;
        entity.IsActive = request.IsActive;

        await _repo.UpdateAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<StoreResponse>.Ok(new StoreResponse { Id = entity.Id, Name = entity.Name, Address = entity.Address, Phone = entity.Phone, OpenTime = entity.OpenTime.ToString(@"hh\:mm"), CloseTime = entity.CloseTime.ToString(@"hh\:mm"), GoogleMapUrl = entity.GoogleMapUrl });
    }

    public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return ApiResponse<bool>.Fail("Store not found");

        await _repo.DeleteAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true);
    }
}

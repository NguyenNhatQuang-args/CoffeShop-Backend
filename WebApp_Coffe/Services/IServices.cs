using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_Coffe.DTOs;
using WebApp_Coffe.DTOs.Common;

namespace WebApp_Coffe.Services;

public interface ICategoryService
{
    Task<ApiResponse<List<CategoryResponse>>> GetAllActiveAsync();
    Task<ApiResponse<CategoryDetailResponse>> GetBySlugAsync(string slug);
    Task<ApiResponse<CategoryResponse>> CreateAsync(CategoryRequest request);
    Task<ApiResponse<CategoryResponse>> UpdateAsync(Guid id, CategoryRequest request);
    Task<ApiResponse<bool>> DeleteAsync(Guid id);
}

public interface IProductService
{
    Task<ApiResponse<PagedResult<ProductResponse>>> GetProductsAsync(Guid? categoryId, string? tag, string? search, bool? isFeature, int page, int pageSize);
    Task<ApiResponse<ProductDetailResponse>> GetBySlugAsync(string slug);
    Task<ApiResponse<List<ProductResponse>>> GetFeaturedAsync(int count);
    
    Task<ApiResponse<ProductResponse>> CreateAsync(ProductRequest request);
    Task<ApiResponse<ProductResponse>> UpdateAsync(Guid id, ProductRequest request);
    Task<ApiResponse<bool>> DeleteAsync(Guid id);
}

public interface IBlogService
{
    Task<ApiResponse<PagedResult<BlogPostResponse>>> GetPublishedBlogsAsync(int page, int pageSize);
    Task<ApiResponse<BlogPostDetailResponse>> GetBySlugAsync(string slug);
    
    Task<ApiResponse<BlogPostResponse>> CreateAsync(BlogPostRequest request);
    Task<ApiResponse<BlogPostResponse>> UpdateAsync(Guid id, BlogPostRequest request);
    Task<ApiResponse<bool>> DeleteAsync(Guid id);
}

public interface IStoreService
{
    Task<ApiResponse<List<StoreResponse>>> GetAllActiveAsync();
    Task<ApiResponse<StoreResponse>> CreateAsync(StoreRequest request);
    Task<ApiResponse<StoreResponse>> UpdateAsync(Guid id, StoreRequest request);
    Task<ApiResponse<bool>> DeleteAsync(Guid id);
}

public interface IAuthService
{
    Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string refreshToken);
}

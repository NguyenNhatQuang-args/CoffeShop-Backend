using System;
using Microsoft.AspNetCore.Http;

namespace WebApp_Coffe.DTOs;

public class BlogPostResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Thumbnail { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class BlogPostDetailResponse : BlogPostResponse
{
    public string Content { get; set; } = string.Empty;
}

public class BlogPostRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Thumbnail { get; set; }
    public IFormFile? ThumbnailFile { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
}

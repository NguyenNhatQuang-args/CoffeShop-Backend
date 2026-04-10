using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp_Coffe.DTOs;
using WebApp_Coffe.Services;

namespace WebApp_Coffe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly IBlogService _service;

    public BlogController(IBlogService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetBlogs([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetPublishedBlogsAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var result = await _service.GetBySlugAsync(slug);
        return result.Success ? Ok(result) : NotFound(result);
    }
}

[ApiController]
[Route("api/admin/blog")]
[Authorize(Roles = "Admin")]
public class AdminBlogController : ControllerBase
{
    private readonly IBlogService _service;

    public AdminBlogController(IBlogService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] BlogPostRequest request)
    {
        var result = await _service.CreateAsync(request);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] BlogPostRequest request)
    {
        var result = await _service.UpdateAsync(id, request);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp_Coffe.DTOs;
using WebApp_Coffe.Services;

namespace WebApp_Coffe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] Guid? categoryId, [FromQuery] string? tag, [FromQuery] string? search, [FromQuery] bool? isFeature, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetProductsAsync(categoryId, tag, search, isFeature, page, pageSize);
        return Ok(result);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var result = await _service.GetBySlugAsync(slug);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet("featured")]
    public async Task<IActionResult> GetFeatured()
    {
        var result = await _service.GetFeaturedAsync(10);
        return Ok(result);
    }
}

[ApiController]
[Route("api/admin/products")]
[Authorize(Roles = "Admin")]
public class AdminProductsController : ControllerBase
{
    private readonly IProductService _service;

    public AdminProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ProductRequest request)
    {
        var result = await _service.CreateAsync(request);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] ProductRequest request)
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

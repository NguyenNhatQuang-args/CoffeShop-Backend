using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp_Coffe.DTOs.Common;
using WebApp_Coffe.Services;

namespace WebApp_Coffe.Controllers;

[ApiController]
[Route("api/admin/upload")]
[Authorize(Roles = "Admin")]
public class UploadController : ControllerBase
{
    private readonly IFileUploadService _uploadService;

    public UploadController(IFileUploadService uploadService)
    {
        _uploadService = uploadService;
    }

    [HttpPost("product")]
    public async Task<IActionResult> UploadProductImage([FromForm] IFormFile file)
    {
        try
        {
            var url = await _uploadService.UploadAsync(file, "products");
            return Ok(ApiResponse<string>.Ok(url, "Product image uploaded successfully"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }

    [HttpPost("category")]
    public async Task<IActionResult> UploadCategoryImage([FromForm] IFormFile file)
    {
        try
        {
            var url = await _uploadService.UploadAsync(file, "categories");
            return Ok(ApiResponse<string>.Ok(url, "Category image uploaded successfully"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }

    [HttpPost("blog")]
    public async Task<IActionResult> UploadBlogThumbnail([FromForm] IFormFile file)
    {
        try
        {
            var url = await _uploadService.UploadAsync(file, "blogs");
            return Ok(ApiResponse<string>.Ok(url, "Blog thumbnail uploaded successfully"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }

    [HttpDelete]
    public IActionResult DeleteFile([FromQuery] string fileUrl)
    {
        var result = _uploadService.DeleteFile(fileUrl);
        if (result)
            return Ok(ApiResponse<bool>.Ok(true, "File deleted successfully"));
        
        return NotFound(ApiResponse<bool>.Fail("File not found or invalid URL"));
    }
}

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp_Coffe.DTOs;
using WebApp_Coffe.DTOs.Common;
using WebApp_Coffe.Services;

namespace WebApp_Coffe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _service.LoginAsync(request);
        if (!result.Success) return Unauthorized(result);

        SetTokenCookies(result.Data!.Token, result.Data.RefreshToken!);

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken)) return Unauthorized(ApiResponse<AuthResponse>.Fail("No refresh token found"));

        var result = await _service.RefreshTokenAsync(refreshToken);
        if (!result.Success) return Unauthorized(result);

        SetTokenCookies(result.Data!.Token, result.Data.RefreshToken!);

        return Ok(result);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None
        };
        Response.Cookies.Delete("jwtToken", cookieOptions);
        Response.Cookies.Delete("refreshToken", cookieOptions);
        return Ok(new { success = true, message = "Logged out successfully" });
    }

    private void SetTokenCookies(string accessToken, string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddHours(2)
        };
        Response.Cookies.Append("jwtToken", accessToken, cookieOptions);

        var refreshOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", refreshToken, refreshOptions);
    }
}

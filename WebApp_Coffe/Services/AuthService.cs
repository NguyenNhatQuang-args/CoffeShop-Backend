using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApp_Coffe.DTOs;
using WebApp_Coffe.DTOs.Common;
using WebApp_Coffe.Repositories;

namespace WebApp_Coffe.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepo;

    public AuthService(IConfiguration config, IUserRepository userRepo)
    {
        _config = config;
        _userRepo = userRepo;
    }

    public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _userRepo.GetByEmailAsync(request.Username); // Assuming Username is Email
        
        if (user != null && user.IsActive && BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            var token = GenerateJwtToken(user.Email, user.Role);
            var refreshToken = GenerateRefreshToken();

            var jwtSettings = _config.GetSection("JwtSettings");
            var refreshExpiryDays = jwtSettings.GetValue<int>("RefreshTokenExpiryDays", 7);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshExpiryDays);
            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveChangesAsync();

            return ApiResponse<AuthResponse>.Ok(new AuthResponse { Token = token, RefreshToken = refreshToken }, "Login successful");
        }
        
        return ApiResponse<AuthResponse>.Fail("Invalid credentials");
    }

    public async Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
            return ApiResponse<AuthResponse>.Fail("Invalid refresh token");

        var user = await _userRepo.GetByRefreshTokenAsync(refreshToken);

        if (user == null || !user.IsActive || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return ApiResponse<AuthResponse>.Fail("Invalid or expired refresh token");
        }

        var newAccessToken = GenerateJwtToken(user.Email, user.Role);
        var newRefreshToken = GenerateRefreshToken();

        var jwtSettings = _config.GetSection("JwtSettings");
        var refreshExpiryDays = jwtSettings.GetValue<int>("RefreshTokenExpiryDays", 7);

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshExpiryDays);
        await _userRepo.UpdateAsync(user);
        await _userRepo.SaveChangesAsync();

        return ApiResponse<AuthResponse>.Ok(new AuthResponse { Token = newAccessToken, RefreshToken = newRefreshToken }, "Token refreshed successfully");
    }

    private string GenerateJwtToken(string username, string role)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "super_secret_key_12345678901234567890"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expiryMinutes = jwtSettings.GetValue<int>("AccessTokenExpiryMinutes", 15);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"] ?? "CoffeeShopApi",
            audience: jwtSettings["Audience"] ?? "CoffeeShopApi",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}

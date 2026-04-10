using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace WebApp_Coffe.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IWebHostEnvironment _environment;

    public FileUploadService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> UploadAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");

        if (file.Length > 5 * 1024 * 1024)
            throw new ArgumentException("File size exceeds 5MB limit");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(ext))
            throw new ArgumentException("Invalid file extension. Only jpg, jpeg, png, webp are allowed.");

        var webRootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadsFolder = Path.Combine(webRootPath, "uploads", folder);

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}{ext}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return $"/uploads/{folder}/{uniqueFileName}";
    }

    public bool DeleteFile(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl) || !fileUrl.StartsWith("/uploads/"))
            return false;

        var relativePath = fileUrl.TrimStart('/');
        var webRootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var filePath = Path.Combine(webRootPath, relativePath);
        
        // Ensure path uses correct directory separator for OS
        filePath = filePath.Replace('/', Path.DirectorySeparatorChar);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            return true;
        }

        return false;
    }
}

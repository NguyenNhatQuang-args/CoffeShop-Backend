using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApp_Coffe.Services;

public interface IFileUploadService
{
    Task<string> UploadAsync(IFormFile file, string folder);
    bool DeleteFile(string fileUrl);
}

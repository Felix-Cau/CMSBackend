using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces
{
    public interface IFileHandler
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
}

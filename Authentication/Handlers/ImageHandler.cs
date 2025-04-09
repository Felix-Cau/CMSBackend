using Microsoft.AspNetCore.Http;

namespace Authentication.Handlers
{
    public class ImageHandler(string imagePath)
    {
        private readonly string _imagePath = imagePath;

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file is null || file.Length == 0)
                return null;

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"f{Guid.NewGuid()}{extension}";

            // Detta kommer att bytas ut mot Azure Blob storage i videon
            if (!Directory.Exists(_imagePath))
                Directory.CreateDirectory(_imagePath);

            var filePath = Path.Combine(_imagePath, extension);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            //Här tar skiten slut som ska byta ut.

            return fileName;
        }
    }
}

﻿using System.Reflection.Metadata;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Domain.Handlers
{
    public class AzureFileHandler(string connectionString, string containerName) : IFileHandler
    {
        private readonly BlobContainerClient
            _containerClient = new(connectionString, containerName);

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file is null || file.Length == 0)
                return null;

            var fileExtension = Path.GetExtension(file.Name);
            var fileName = $"f{Guid.NewGuid()}{fileExtension}";

            var contentType = !string.IsNullOrEmpty(file.ContentType)
                ? file.ContentType
                : "application/octet-stream";

            if ((contentType == "application/octet-stream" || string.IsNullOrEmpty(contentType)) &&
                fileExtension.Equals(".svg", StringComparison.OrdinalIgnoreCase))
                contentType = "image/svg+xml";

            BlobClient blobClient = _containerClient.GetBlobClient(fileName);
            var uploadOptions = new BlobUploadOptions
                { HttpHeaders = new BlobHttpHeaders { ContentType = contentType } };

            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, uploadOptions);

            return blobClient.Uri.ToString();

        }
    }
}

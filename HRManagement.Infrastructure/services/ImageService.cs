
using HRManagement.Application.Interfaces;

namespace HRManagement.Infrastructure.Services
{
    public class ImageService(IWebHostEnvironment environment) : IImageService
    {
        private readonly IWebHostEnvironment _environment = environment;
        private readonly IEnumerable<string> _allowedExtensions = [".jpg", ".jpeg", ".png", ".gif"];
        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB - consistent with global FormOptions limit

        public async Task<(string filePath, string fileName)> SaveImage(Stream file, string folder, string originalFileName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be null");

            if (!IsValidImage(file, originalFileName))
                throw new ArgumentException("Invalid image file");

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folder);

            // Ensure directory exists
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = GenerateUniqueFileName(originalFileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                using var fileStream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(fileStream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save image: {ex.Message}", ex);
            }

            var relativePath = Path.Combine("uploads", folder, uniqueFileName).Replace('\\', '/');
            return (relativePath, uniqueFileName);
        }

        public void DeleteImage(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            var fullPath = Path.Combine(_environment.WebRootPath, filePath.Replace('/', '\\'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public bool IsValidImage(Stream file, string fileName)
        {
            if (file == null || file.Length == 0) return false;
            if (file.Length > MaxFileSize) return false;

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return _allowedExtensions.Contains(extension);
        }

        private static string GenerateUniqueFileName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            var newName = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            return $"{newName}_{Guid.NewGuid()}{extension}";
        }
    }
}
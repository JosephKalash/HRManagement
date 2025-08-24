
namespace HRManagement.Application.Interfaces
{
    public interface IImageService
    {
        Task<(string filePath, string fileName)> SaveImage(Stream imageStream, string folderName, string originalFileName);
        void DeleteImage(string filePath);
        bool IsValidImage(Stream imageStream, string fileName);
    }
}
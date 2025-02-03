using System.Threading.Tasks;

namespace UdemyCarBook.Application.Interfaces.IService
{
    public interface IImageUploadService
    {
        Task<string> UploadImageAsync(string base64Image, string folderName);
        Task<bool> DeleteImageAsync(string imagePath);
        string GetImageUrl(string imagePath);
    }
} 
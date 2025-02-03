using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Persistance.Service
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadsFolder = "uploads";
        private readonly ILogService _logService;

        public ImageUploadService(IWebHostEnvironment environment, ILogService logService)
        {
            _environment = environment;
            _logService = logService;
        }

        public async Task<string> UploadImageAsync(string base64Image, string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty(base64Image))
                {
                    await _logService.CreateLog(
                        "Resim Yükleme Hatası",
                        "Base64 string boş veya null",
                        "Error",
                        "ImageUpload"
                    );
                    throw new AuFrameWorkException(
                        "Resim verisi boş olamaz",
                        "IMAGE_DATA_EMPTY",
                        "Error"
                    );
                }

                // Base64'ü temizle
                var base64Data = base64Image.Contains("base64,") 
                    ? base64Image.Split("base64,")[1]
                    : base64Image;

                try
                {
                    // Base64'ü byte dizisine çevir
                    byte[] imageBytes = Convert.FromBase64String(base64Data);

                    // Uploads klasörü yolunu oluştur
                    var uploadsPath = Path.Combine(_environment.WebRootPath, _uploadsFolder);
                    
                    // Belirtilen alt klasör yolunu oluştur
                    var folderPath = Path.Combine(uploadsPath, folderName);

                    // Klasörler yoksa oluştur
                    if (!Directory.Exists(uploadsPath))
                        Directory.CreateDirectory(uploadsPath);
                    
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    // Benzersiz dosya adı oluştur
                    var fileName = $"{Guid.NewGuid()}.jpg";
                    var filePath = Path.Combine(folderPath, fileName);

                    // Dosyayı kaydet
                    await File.WriteAllBytesAsync(filePath, imageBytes);

                    // Veritabanında saklanacak göreceli yolu döndür
                    return Path.Combine(_uploadsFolder, folderName, fileName).Replace("\\", "/");
                }
                catch (FormatException)
                {
                    await _logService.CreateLog(
                        "Resim Yükleme Hatası",
                        "Geçersiz Base64 formatı",
                        "Error",
                        "ImageUpload"
                    );
                    throw new AuFrameWorkException(
                        "Geçersiz resim formatı",
                        "INVALID_IMAGE_FORMAT",
                        "Error"
                    );
                }
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "ImageUpload",
                    "Resim yüklenirken beklenmeyen bir hata oluştu"
                );
                throw new AuFrameWorkException(
                    "Resim yüklenirken bir hata oluştu",
                    "IMAGE_UPLOAD_ERROR",
                    "Error"
                );
            }
        }

        public async Task<bool> DeleteImageAsync(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                    return false;

                var fullPath = Path.Combine(_environment.WebRootPath, imagePath);

                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "ImageDelete",
                    "Resim silinirken hata oluştu"
                );
                return false;
            }
        }

        public string GetImageUrl(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return null;

            return $"/{imagePath.Replace("\\", "/")}";
        }
    }
} 
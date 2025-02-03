using System.Threading.Tasks;

namespace UdemyCarBook.Application.Interfaces.IService
{
    public interface IPasswordHashService
    {
        Task<(string passwordHash, string passwordSalt)> HashPasswordAsync(string password);
        Task<bool> VerifyPasswordAsync(string password, string passwordHash, string passwordSalt);
    }
} 
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UdemyCarBook.Application.Interfaces.IService;

namespace UdemyCarBook.Persistance.Service
{
    public class PasswordHashService : IPasswordHashService
    {
        public async Task<(string passwordHash, string passwordSalt)> HashPasswordAsync(string password)
        {
            using var hmac = new HMACSHA512();
            var passwordSalt = Convert.ToBase64String(hmac.Key);
            var passwordHash = Convert.ToBase64String(
                hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
            );

            return (passwordHash, passwordSalt);
        }

        public async Task<bool> VerifyPasswordAsync(string password, string passwordHash, string passwordSalt)
        {
            using var hmac = new HMACSHA512(Convert.FromBase64String(passwordSalt));
            var computedHash = Convert.ToBase64String(
                hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
            );

            return computedHash == passwordHash;
        }
    }
} 
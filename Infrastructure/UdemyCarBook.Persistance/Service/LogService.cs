using System;
using System.Threading.Tasks;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Persistance.Context;

namespace UdemyCarBook.Persistance.Service
{
    public class LogService : ILogService
    {
        private readonly NewsContext _context;

        public LogService(NewsContext context)
        {
            _context = context;
        }

        public async Task CreateLog(string title, string message, string type, string location)
        {
            var log = new Log
            {
                Id = Guid.NewGuid(),
                Title = title,
                Message = message,
                Type = type,
                Location = location,
                CreatedDate = DateTime.UtcNow
            };

            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task CreateErrorLog(Exception ex, string location, string message)
        {
            var log = new Log
            {
                Id = Guid.NewGuid(),
                Title = "Hata",
                Message = $"{message} - Hata: {ex.Message}",
                Type = "Error",
                Location = location,
                CreatedDate = DateTime.UtcNow
            };

            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task CreateValidationErrorLog(string title, string message, string location)
        {
            var log = new Log
            {
                Id = Guid.NewGuid(),
                Title = title,
                Message = message,
                Type = "ValidationError",
                Location = location,
                CreatedDate = DateTime.UtcNow
            };

            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}

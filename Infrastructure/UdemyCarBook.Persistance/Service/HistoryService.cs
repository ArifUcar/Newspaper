using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Base;
using UdemyCarBook.Persistance.Context;

namespace UdemyCarBook.Persistance.Service
{
    public class HistoryService : IHistoryService
    {
        private readonly NewsContext _context;

        public HistoryService(NewsContext context)
        {
            _context = context;
        }

        public async Task SaveHistory<T>(T entity, string modificationType, string modifiedBy = null) where T : class
        {
            var entityType = typeof(T);
            var idProperty = entityType.GetProperty("Id");
            var entityId = idProperty?.GetValue(entity);

            var history = new BaseHistory
            {
                Id = Guid.NewGuid(),
                EntityId = (Guid)entityId,
                EntityName = entityType.Name,
                EntityData = JsonSerializer.Serialize(entity),
                ModifiedDate = DateTime.Now,
                ModificationType = modificationType,
                ModifiedBy = modifiedBy
            };

            await _context.Histories.AddAsync(history);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BaseHistory>> GetHistory<T>(Guid entityId) where T : class
        {
            return await _context.Histories
                .Where(x => x.EntityId == entityId && x.EntityName == typeof(T).Name)
                .OrderByDescending(x => x.ModifiedDate)
                .ToListAsync();
        }

        public async Task<List<BaseHistory>> GetHistoryByEntityName(string entityName)
        {
            return await _context.Histories
                .Where(x => x.EntityName == entityName)
                .OrderByDescending(x => x.ModifiedDate)
                .ToListAsync();
        }
    }
}

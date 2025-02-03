using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyCarBook.Domain.Base;

namespace UdemyCarBook.Application.Interfaces.IService
{
    public interface IHistoryService
    {
        Task SaveHistory<T>(T entity, string modificationType, string modifiedBy = null) where T : class;
        Task<List<BaseHistory>> GetHistory<T>(Guid entityId) where T : class;
        Task<List<BaseHistory>> GetHistoryByEntityName(string entityName);
    }
}

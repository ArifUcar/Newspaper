using System;
using System.Threading.Tasks;

namespace UdemyCarBook.Application.Interfaces.IService
{
    public interface ILogService
    {
        Task CreateLog(string title, string message, string type, string location);
        Task CreateErrorLog(Exception ex, string location, string message);
        Task CreateValidationErrorLog(string title, string message, string location);
    }
}

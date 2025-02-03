using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.WebApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public ExceptionMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            
            var errorResponse = new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "Internal Server Error",
                DetailedMessage = ex.Message,
                Path = context.Request.Path,
                Method = context.Request.Method,
                Timestamp = DateTime.Now,
                TraceId = context.TraceIdentifier
            };

            // CarBookException için özel işleme
            if (ex is AuFrameWorkException auFrameWorkException)
            {
                errorResponse.StatusCode = auFrameWorkException.ErrorType switch
                {
                    "NotFound" => (int)HttpStatusCode.NotFound,
                    "ValidationError" => (int)HttpStatusCode.BadRequest,
                    "Unauthorized" => (int)HttpStatusCode.Unauthorized,
                    "Forbidden" => (int)HttpStatusCode.Forbidden,
                    "Conflict" => (int)HttpStatusCode.Conflict,
                    "TooManyRequests" => (int)HttpStatusCode.TooManyRequests,
                    _ => (int)HttpStatusCode.InternalServerError
                };
                
                errorResponse.Message = auFrameWorkException.ErrorType;
                errorResponse.ErrorCode = auFrameWorkException.ErrorCode;
            }
            // Diğer özel exception tipleri için işlemler
            else if (ex is UnauthorizedAccessException)
            {
                errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.Message = "Unauthorized";
                errorResponse.ErrorCode = "UNAUTHORIZED_ACCESS";
            }
            else if (ex is ArgumentException)
            {
                errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = "Invalid Arguments";
                errorResponse.ErrorCode = "INVALID_ARGUMENTS";
            }

            context.Response.StatusCode = errorResponse.StatusCode;

            // Scoped servis için yeni scope oluştur
            using (var scope = _scopeFactory.CreateScope())
            {
                var logService = scope.ServiceProvider.GetRequiredService<ILogService>();

                var logDetail = new
                {
                    Exception = new
                    {
                        Message = ex.Message,
                        StackTrace = ex.StackTrace,
                        Source = ex.Source,
                        InnerException = ex.InnerException?.Message
                    },
                    Request = new
                    {
                        Path = context.Request.Path,
                        Method = context.Request.Method,
                        QueryString = context.Request.QueryString.ToString(),
                        Headers = context.Request.Headers
                            .Where(h => !h.Key.StartsWith("sec-"))
                            .ToDictionary(h => h.Key, h => h.Value.ToString())
                    },
                    Response = new
                    {
                        StatusCode = errorResponse.StatusCode,
                        ErrorCode = errorResponse.ErrorCode,
                        Message = errorResponse.Message
                    },
                    TraceId = context.TraceIdentifier,
                    Timestamp = DateTime.Now
                };

                // Hata detaylarını logla
                await logService.CreateErrorLog(
                    ex,
                    context.Request.Path,
                    JsonSerializer.Serialize(logDetail)
                );
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }
    }

    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string DetailedMessage { get; set; }
        public string ErrorCode { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public string TraceId { get; set; }
        public DateTime Timestamp { get; set; }
    }

    // Extension method for easy middleware registration
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
} 
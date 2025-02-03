using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace UdemyCarBook.Application.Attributes
{
    public class AUAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public AUAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Eğer rol belirtilmişse, kullanıcının rollerini kontrol et
            if (_roles != null && _roles.Length > 0)
            {
                var userRoles = user.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList();

                var hasRequiredRole = _roles.Any(role => userRoles.Contains(role));
                if (!hasRequiredRole)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }

            // Token'dan kullanıcı bilgilerini al
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = user.FindFirst(ClaimTypes.Name)?.Value;
            var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;
            var userType = user.FindFirst("UserType")?.Value;

            // Kullanıcı bilgilerini header'lara ekle
            context.HttpContext.Response.Headers.Add("X-UserId", userId ?? "");
            context.HttpContext.Response.Headers.Add("X-UserName", userName ?? "");
            context.HttpContext.Response.Headers.Add("X-UserEmail", userEmail ?? "");
            context.HttpContext.Response.Headers.Add("X-UserType", userType ?? "");
        }
    }
} 
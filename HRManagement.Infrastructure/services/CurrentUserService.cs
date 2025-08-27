using System.Security.Claims;
using HRManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HRManagement.Infrastructure.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public long? UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                var idClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? user?.FindFirst("sub")?.Value
                              ?? user?.FindFirst("uid")?.Value;
                if (long.TryParse(idClaim, out var id))
                {
                    return id;
                }
                return null;
            }
        }

        public string? UserName
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                return user?.Identity?.Name
                       ?? user?.FindFirst(ClaimTypes.Name)?.Value
                       ?? user?.FindFirst("preferred_username")?.Value;
            }
        }
    }
}

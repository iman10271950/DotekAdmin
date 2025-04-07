using Application.Common.Interfaces.Services;
using System.Security.Claims;

namespace WebUI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //here we need to set current user

        public string Username => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public string Name => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

        public string? UserRoleId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Sid);

        public Guid Key => throw new NotImplementedException();

        public string? AlternativeToken => throw new NotImplementedException();

        public string? UrlOwner => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.PrimarySid);

        public string? NationalCode => throw new NotImplementedException();

        public string NationalCodeOwner => throw new NotImplementedException();

        public string RoleTypeOwner => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

       
    }
}

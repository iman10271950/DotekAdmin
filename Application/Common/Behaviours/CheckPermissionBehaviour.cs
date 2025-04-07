using Application.Common.Interfaces.Auth;
using Application.Common.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Auth;
using Microsoft.AspNetCore.Http.Features;
using DocumentFormat.OpenXml.InkML;


namespace Application.Common.Behaviours
{
    public class CheckPermissionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUser;
        private readonly IDistributedCache _cache;
        private readonly ICheckPermissionPointer _checkPermissionPointer;
        private readonly ITokenService _tokenService;

        public CheckPermissionBehaviour(IHttpContextAccessor httpContextAccessor,
                                        ICurrentUserService currentUser,
                                        IDistributedCache cache,
                                        ICheckPermissionPointer checkPermissionPointer,
                                        ITokenService tokenService)
        {
            _httpContextAccessor = httpContextAccessor;
            _currentUser = currentUser;
            _cache = cache;
            _checkPermissionPointer = checkPermissionPointer;
            _tokenService = tokenService;
            _checkPermissionPointer.Pointer = false;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_checkPermissionPointer.Pointer == true)
            {
                return await next();
            }
            var httpContext = _httpContextAccessor.HttpContext;
            var authorizeAttributeList = GetAttribute<DotekAuthorizeAttribute>(httpContext);

            if (authorizeAttributeList == null || authorizeAttributeList.Count == 0)
            {
                return await next();
            }
            else
            {
                string[] policies = authorizeAttributeList.Select(attr => attr.Policy).ToArray();
                var userAccessList = await _tokenService.GetUserAccesses(_currentUser.UserRoleId);

                var hasAccess = true;
                foreach (var policy in policies)
                {
                    hasAccess = hasAccess && userAccessList.Any(x => x.Id.ToString() == policy);
                }
                _checkPermissionPointer.Pointer = true;
                if (hasAccess)
                {
                    new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                   .RequireClaim(ClaimTypes.Sid, _currentUser.UserRoleId)
                   .Build();
                    return await next();
                }
                throw new Exception("شما به این عملیات دسترسی ندارید");

            }
        }

        private List<T>? GetAttribute<T>(HttpContext httpContext) where T : Attribute
        {
           

            return httpContext.Features?.Get<IEndpointFeature>()?.Endpoint?.Metadata
            .Where(m => m.GetType() == typeof(T))
            .Cast<T>()
            .ToList();
        }
    }
}

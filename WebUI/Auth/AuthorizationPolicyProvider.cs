using Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace WebUI.Auth
{
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IDistributedCache _cashe;
        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, ICurrentUserService currentUser, IDistributedCache cache)
            : base(options)
        {
            _currentUser = currentUser;
            _cashe = cache;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            //string key = $"UserRoleAccess:{{{_currentUser.UserRoleId}}}";
            //UserAccessList_VM userAccessList = await _cashe.GetRecordAsync<UserAccessList_VM>(key);
            //if (userAccessList == null)
            //{
            //    userAccessList = await new User_SR().GetUserAccessedList(_currentUser.AlternativeToken);
            //    if (userAccessList.Success != true)
            //    {
            //        var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
            //    .RequireClaim(ClaimTypes.NameIdentifier, _currentUser.UserRoleId)
            //    .Build();

            //        return await Task.FromResult(policy);
            //    }
            //    await _cashe.SetRecordAsync<UserAccessList_VM>(key, userAccessList, new TimeSpan(0, 30, 0));
            //}

            //var hasAccess = userAccessList.List.Any(x => x.ActionCode == long.Parse(policyName));
            //if (hasAccess)
            //{
            //    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
            //    .RequireClaim(ClaimTypes.NameIdentifier, _currentUser.UserRoleId)
            //    .Build();

            //    return await Task.FromResult(policy);
            //}
            //return await base.GetPolicyAsync(policyName);
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                .RequireClaim(ClaimTypes.Sid, _currentUser.UserRoleId)
                .Build();

            return await Task.FromResult(policy);
            //  return await Task.FromResult(new AuthorizationPolicyBuilder().RequireAssertion(x => true).Build());

        }
    }
}

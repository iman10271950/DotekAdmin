
using Application.Common.BaseEntities;
using Application.Common.Extentions;
using Application.Common.Interfaces.Services;
using Application.Common.InterFaces.DbContext;
using Domain.Enums.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business.Auth.User.Query
{
    public class LogoutUserQuery : IRequest<BaseResult_VM<string>>
    {
    }
    public class LogoutUserQueryHandler : IRequestHandler<LogoutUserQuery, BaseResult_VM<string>>
    {
        private readonly IAdminDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDistributedCache _cache;

        public LogoutUserQueryHandler(IAdminDbContext dbContext, IHttpContextAccessor httpContextAccessor, ICurrentUserService currentUserService, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = currentUserService;
            _cache = cache;
        }
        public async Task<BaseResult_VM<string>> Handle(LogoutUserQuery request, CancellationToken cancellationToken)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var userId = int.Parse(_currentUserService.UserRoleId);

            if (string.IsNullOrEmpty(token))
            {
                return new BaseResult_VM<string>("false", -1, "توکن شما یافت نشد ");
            }

            await _cache.DeleteRecordAsync(userId.ToString());

            UpdateSession(userId, token, cancellationToken);

            return new BaseResult_VM<string>("true", 0, "شما با موفقیت از سیستم خارج شده اید");
        }
        private async Task<bool> UpdateSession(int userId, string token, CancellationToken cancellationToken)
        {

            var rowsAffected = await Microsoft.EntityFrameworkCore.RelationalQueryableExtensions.ExecuteUpdateAsync(
         _dbContext.Session
             .Where(m => m.Token == token && m.Status == (int)SessionStatus.Active && m.UserId == userId),
         m => m.SetProperty(x => x.Status, (int)SessionStatus.Inactive),
         cancellationToken
     );

            return rowsAffected > 0;
        }
    }
}

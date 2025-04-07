using Application.Common.Extentions;
using Application.Common.Interfaces.Auth;
using Application.Common.Interfaces.Services;
using Application.Common.InterFaces.DbContext;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class UserSessionBehaviour<TRequest, Tresponse> : IPipelineBehavior<TRequest, Tresponse> where TRequest : IRequest<Tresponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserService _currentUserService;
        private readonly ITokenService _tokenService;
        private readonly IDistributedCache _cache;
        private readonly IExecutionMode _executionMode;
        private readonly IConnectionMultiplexer _connMultiplexer;

        public UserSessionBehaviour(IHttpContextAccessor httpContextAccessor,
                                    IAdminDbContext dbcontext, IConfiguration configuration, ICurrentUserService currentUserService,
                                    ITokenService tokenService, IDistributedCache cache, IExecutionMode executionMode,
                                    IConnectionMultiplexer connectionMultiplexer)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbcontext;
            _configuration = configuration;
            _currentUserService = currentUserService;
            _tokenService = tokenService;
            _cache = cache;
            _executionMode = executionMode;
            _connMultiplexer = connectionMultiplexer;
        }
        public async Task<Tresponse> Handle(TRequest request, RequestHandlerDelegate<Tresponse> next, CancellationToken cancellationToken)
        {

            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            if (!string.IsNullOrEmpty(token))
            {
                if (_currentUserService.UserRoleId == null)
                    throw new Exception("کاربر گرامی نشست شما منقضی شده است");

                if (_executionMode.Mode == Domain.Enums.Auth.ExecutionModeEnum.Production)
                {
                    var jwtToken = await _cache.GetRecordAsync<string>(_currentUserService.UserRoleId);

                    if (jwtToken == null)
                    {
                        throw new Exception("کاربر گرامی نشست شما منقضی شده است");
                    }

                    if (jwtToken == token)
                    {
                        return await next();
                    }
                    else
                    {
                        throw new Exception("کاربر گرامی نشست شما نامعتبر است");
                    }
                }

                //var session = await _dbContext.Session
                //    .Where(m => m.Token == token)
                //    .FirstOrDefaultAsync();

                //if (session == null || session!.Status == (int)Status.Inactive) throw new ArkaUnauthorizedAccessException("کاربر گرامی نشست شما منقضی شده است");

                //if (session.Status == (int)Status.Active && session.ExpiresAt < DateTime.Now)
                //{
                //    session.Status = (int)Status.Inactive;
                //    if (await UpdateSession(session, cancellationToken) == false) throw new ArkaDBOperationException("کاربر گرامی در ذخیره سازی اطلاعات خطایی رخ داده است");
                //    throw new ArkaUnauthorizedAccessException("کاربر گرامی نشست شما منقضی شده است");
                //}

                //if (session.IssuedAt.AddMinutes(1) < DateTime.Now && session.ExpiresAt > DateTime.Now)
                //{
                //    long userId = long.Parse(_currentUserService.UserId);
                //    var tokenResponse = await _tokenService.CreateToken(new Auth.CreateTokenRequest(userId, _currentUserService.Username, _currentUserService.Name));

                //    session.Token = tokenResponse.Token;
                //    session.IssuedAt = DateTime.Now;

                //    if (await UpdateSession(session, cancellationToken) == false) throw new ArkaDBOperationException("کاربر گرامی در ذخیره سازی اطلاعات خطایی رخ داده است");

                //    _httpContextAccessor.HttpContext.Response.Headers.Add("Authorization", $"Bearer {tokenResponse.Token}");

                //}
                //else
                //{
                //    _httpContextAccessor.HttpContext.Response.Headers.Add("Authorization", $"Bearer {session.Token}");
                //}

            }

            return await next();
        }

        //private async Task<bool> UpdateSession(Domain.Entities.Auth.Session session, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        _dbContext.Session.Update(session);
        //        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}


    }
}

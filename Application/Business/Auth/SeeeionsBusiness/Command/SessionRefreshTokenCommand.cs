using Application.Business.Auth.SeeeionsBusiness.ViewModel;
using Application.Business.Common.ViewModel;
using Application.Common.Auth;
using Application.Common.BaseEntities;
using Application.Common.Extentions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Auth;
using Application.Common.Interfaces.Services;
using Application.Common.InterFaces.DbContext;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entities.Auth;
using Domain.Enum;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Auth.Session.Command
{
    public class SessionRefreshTokenCommand : IRequest<BaseResult_VM<Token_VM>>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public class SessionRefreshTokenHandler : IRequestHandler<SessionRefreshTokenCommand, BaseResult_VM<Token_VM>>
    {
        private readonly IAdminDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly IDistributedCache _cache;
        private readonly ICurrentUserService _userService;

        public SessionRefreshTokenHandler(IAdminDbContext dbContext, ITokenService tokenService, IDistributedCache cache, ICurrentUserService userService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _cache = cache;
            _userService = userService;
        }
        public async Task<BaseResult_VM<Token_VM>> Handle(SessionRefreshTokenCommand request, CancellationToken cancellationToken)
        {

            int userRoleId = 0;
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Username == _userService.Username);
            int.TryParse(_userService.UserRoleId, out userRoleId);

            var token = await _cache.GetRecordAsync<string>(_userService.UserRoleId);

            if (string.IsNullOrEmpty(token))
                return new BaseResult_VM<Token_VM>(null, 1, "کاربر گرامی نشست فعالی برای تمدید یافت نشد");

            var redisSession = JsonConvert.DeserializeObject<RedisSession>(token);

            //بررسی تطابق رفرش توکن ارسالی و موجود در ردیس
            if (redisSession.RefreshToken == request.RefreshToken)
            {
                var absoluteExpireTimeMin = (int)(redisSession.AbsoluteExpirationTime - DateTime.Now).TotalMinutes;


                //بررسی زمان باقیمانده برای تمدید
                if (!(absoluteExpireTimeMin > 0))

                    return new BaseResult_VM<Token_VM>(null, 2, "کاربر گرامی تمدید نشست امکان پذیر نمیباشد");

                var absoulteDatetime = DateTime.Now.AddMinutes(absoluteExpireTimeMin);
               
                //ایجاد JWT 
                var newJWTToken = await _tokenService.CreateToken(new CreateTokenRequest(userRoleId, _userService.Username, _userService.Name), absoulteDatetime);
                //ایجاد refreshToken
                var newRefreshToken = await _tokenService.CreateRefreshToken();

                var adminSettings = await _dbContext.AdminSetting.FirstOrDefaultAsync();


                RedisSession newRedisSession = new RedisSession { JWTToken = newJWTToken.Token, RefreshToken = newRefreshToken, AbsoluteExpirationTime = absoulteDatetime };
                var serializedSession = JsonConvert.SerializeObject(newRedisSession);

                //پاک کردن رکورد قبلی
                await _cache.DeleteRecordAsync(userRoleId.ToString());
                //ست کردن توکن و رفرش توکن در ردیس
                await _cache.SetRecordAsync(userRoleId.ToString(), serializedSession, TimeSpan.FromMinutes(absoluteExpireTimeMin), TimeSpan.FromMinutes(adminSettings.UserJWTTokenExpireTime));


                UpdateSession(redisSession.JWTToken, redisSession.RefreshToken, newJWTToken.Token, newRefreshToken, absoulteDatetime, cancellationToken);


                return new BaseResult_VM<Token_VM>(new Token_VM { Token = newJWTToken.Token, RefreshToken = newRefreshToken }, 0, "توکن با موفقیت بروزرسانی شد");
            }
            else
            {
                return new BaseResult_VM<Token_VM>(null, 2, "کاربر گرامی تمدید نشست امکان پذیر نمیباشد");
            }

            //var session = await _dbContext.Session.Where(m => m.Token == request.Token && m.RefreshToken == request.RefreshToken).FirstOrDefaultAsync();

            //if (session == null) return new BaseResult_VM<Token_VM>(null, -400, "کاربر گرامی توکن یافت نشد");

            //if (session.Status == (int)SessionStatus.Inactive) return new BaseResult_VM<Token_VM>(null, 1, "کاربر گرامی توکن شما منقضی است");

            //if (session.ExpiresAt < DateTime.Now) return new BaseResult_VM<Token_VM>(null, 1, "کاربر گرامی توکن شما منقضی شده است");

            //var user = _dbContext.User.FirstOrDefault(m => m.Id == session.UserId);

            //var token = await _tokenService.CreateToken(new Application.Common.Auth.CreateTokenRequest(user!.Id, user.Username, user.Name));
            //var refreshToken = await _tokenService.CreateRefreshToken();

            //session.Token = token.Token;
            //session.RefreshToken = refreshToken;

            //_dbContext.Session.Update(session);

            //if (await _dbContext.SaveChangesAsync(cancellationToken) <= 0) throw new DBOperationException();


        }


        public async Task<BaseResult_VM<Token_VM>> UpdateSession(string token, string refreshToken, string newToken, string newRefreshToken, DateTime expireDate, CancellationToken cancellation)
        {
            var activeSession = await _dbContext.Session.Where(m => m.Token == token && m.RefreshToken == refreshToken).FirstOrDefaultAsync();

            if (activeSession != null)
            {
                var newSession = activeSession.DeepCopy();

                activeSession.Status = (int)Status.Inactive;

                newSession.Id = Guid.NewGuid();
                newSession.Token = newToken;
                newSession.RefreshToken = newRefreshToken;
                newSession.Status = (int)Status.Active;
                newSession.ExpiresAt = expireDate;


                await _dbContext.Session.AddAsync(newSession);
                _dbContext.Session.Update(activeSession);

                await _dbContext.SaveChangesAsync(cancellation);

                return new BaseResult_VM<Token_VM>(null, 0, "");
            }

            return new BaseResult_VM<Token_VM>(null, 1, "نشستی برای بروزرسانی یافت نشد");


        }
    }
}


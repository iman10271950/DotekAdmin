using Application.Business.Common.ViewModel;
using Application.Common.Auth;
using Application.Common.Interfaces.Auth;
using AutoMapper;
using Domain.Enums.Auth;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Application.Business.Auth.Activation.Query;
using Application.Business.Auth.Activation.Command;
using Application.Common.InterFaces.DbContext;
using Domain.Enum;
using Domain.Common;
using Application.Common.Extentions;
using Application.Common.BaseEntities;

namespace Application.Business.Auth.User.Command
{
    public class LoginWithOPTCommand : IRequest<IBaseResult>
    {
        public string PhoneNumber { get; set; }
        /// <summary>
        /// رمز عبور
        /// </summary>
        /// <example>Admin@123</example>
        public string? Password { get; set; }
        public CreateSession_VM Session { get; set; }
        /// <summary>
        /// کد فعالسازی: بار اول نال فرستاده شود بار دوم با مقدار کد پیامک شده پر شود
        /// </summary>
        /// <example>null</example>
        public int? ActivationCode { get; set; }
        public bool LoginOPT { get; set; }
    }
    public class LoginWithOPTCommandHandler : IRequestHandler<LoginWithOPTCommand, IBaseResult>
    {
        private readonly IAdminDbContext dbContext;
        private readonly IPasswordService passwordService;
        private readonly ITokenService tokenService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediatr;
        private readonly ISMS _sms;
        private readonly IDistributedCache _cache;
        public LoginWithOPTCommandHandler(IAdminDbContext dbContext, IPasswordService passwordService, ITokenService tokenService
        , IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IMediator mediator,
        ISMS sms, IDistributedCache cache)
        {
            this.dbContext = dbContext;
            this.passwordService = passwordService;
            this.tokenService = tokenService;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mediatr = mediator;
            _sms = sms;
            _cache = cache;
        }
        public async Task<IBaseResult> Handle(LoginWithOPTCommand request, CancellationToken cancellationToken)
        {

            //واکشی کاربر
            request.PhoneNumber = request.PhoneNumber.Trim().ToLower();
            var user = await GetUser(request.PhoneNumber);

            if (user == null)
                return new Application.Common.BaseEntities.BaseResult_VM<string>(string.Empty, -200, "کاربر گرامی حساب کاربری با اطلاعات وارد شده وجود ندارد");

            if (user.Status == (int)UserStatus.Inactive)
                return new BaseResult_VM<string>(string.Empty, -200, "کاربر گرامی حساب کاربری شما غیر فعال میباشد. برای فعالسازی با مدیر سیستم تماس بگیرید");

            if (user.Status == (int)UserStatus.PendingAcceptAdmin)
                return new BaseResult_VM<string>(string.Empty, -200, "کاربر گرامی حساب کاربری شما منتظر تایید مدیر سیستم میباشد");

            var adminSettings = await dbContext.AdminSetting.FirstOrDefaultAsync();

            // بررسی لاگین همزمان
            //var sessions = await ExistingSession(user.Id, _mapper, cancellationToken);

            //var maxUsers = adminSettings.MaxUserSessions;
            //if (sessions != null && sessions.Count >= maxUsers)
            //{
            //    return new BaseResult_VM<List<Session_VM>>(sessions, 1, "کاربر گرامی تعداد نشست های فعال شما بیش از حد مجاز است. لطفا از یکی از آنها خارج شوید");
            //}
            if (!request.LoginOPT)
            {
                //بررسی صحت پسورد
                if (!passwordService.ValidatePassword(request.Password, user.Password))
                {
                    return new BaseResult_VM<string>("", -200, "کاربر گرامی رمز عبور وارد شده اشتباه است");
                }
            }



            else
            {
                if (request.ActivationCode.HasValue)
                {
                    var activationResult = (await _mediatr.Send(new GetActivationQuery() { PhoneNumber = user.MobileNumber, Status = (int)Status.Active, ActivationCode = (int)request.ActivationCode })).Result;

                    if (activationResult == null)
                        return new BaseResult_VM<string>("", -200, "کاربر گرامی کد دو عاملی وارد شده اشتباه است");

                    if (activationResult.ExpireTime < DateTime.Now)
                        return new BaseResult_VM<string>("", -200, "کاربر گرامی کد دو عاملی وارد شده منقضی شده است");

                    activationResult.Status = (int)Status.Inactive;

                    dbContext.Activation.Update(activationResult);
                    if (await dbContext.SaveChangesAsync(cancellationToken) <= 0) throw new Exception();

                }
                else
                {
                    // بررسی اجازه ی ارسال کد فعال سازی
                    var activationResult = (await _mediatr.Send(new GetActivationListQuery() { PhoneNumber = user.MobileNumber, Status = (int)Status.Active })).Result;
                    DateTime allowedSMS = DateTime.Now.AddMinutes(3 * -1);
                    if (activationResult != null && activationResult.Where(m => m.CreateTime > allowedSMS).FirstOrDefault() != null)
                    {
                        return new BaseResult_VM<string>("", -200, "کاربر گرامی، در فاصله ی کمتر از سه دقیقه امکان ارسال مجدد رمز دوم وجود ندارد، لطفا پس از این مدت مجددا تلاش فرمایید");
                    }

                    //ارسال sms
                    int code = await GenerateActivationCode();

                    var smsResult = await _sms.Send(new Application.Common.BaseEntities.smsInputSingle { Code = code.ToString(), MpbileNumber = request.PhoneNumber });

                    if (smsResult.Code != 0) { return smsResult; }

                    //درج فعالسازی در دیتابیس
                    var ActivationResult = await _mediatr.Send(new InsertActivationCommand { UserId = (int)user.Id, phoneNumber = user.MobileNumber, code = code }) as BaseResult_VM<string>;
                    if (ActivationResult.Code != 0)
                        return ActivationResult;

                    return new BaseResult_VM<string>("", 2, "پیامک با موفقیت برای شما ارسال شد");
                }
            }

            //پیدا کردن نشست فعال در ردیس
            var jwtToken = await _cache.GetRecordAsync<string>(user.Id.ToString());

            //پاک کردن نشست فعال
            if (!string.IsNullOrEmpty(jwtToken))
            {
                await _cache.DeleteRecordAsync(user.Id.ToString());
            }

            //ایجاد JWT 
            var token = await tokenService.CreateToken(new CreateTokenRequest(user.Id, user.Username, user.Name), DateTime.Now.AddMinutes(adminSettings.UserSessionMaxTime));
            //ایجاد refreshToken
            var refreshToken = await tokenService.CreateRefreshToken();

            RedisSession redisSession = new RedisSession { JWTToken = token.Token, RefreshToken = refreshToken, AbsoluteExpirationTime = DateTime.Now.AddMinutes(adminSettings.UserSessionMaxTime) };
            var serializedSession = JsonConvert.SerializeObject(redisSession);

            //ست کردن توکن و رفرش توکن در ردیس
            await _cache.SetRecordAsync(user.Id.ToString(), serializedSession, TimeSpan.FromMinutes(adminSettings.UserSessionMaxTime), TimeSpan.FromMinutes(adminSettings.UserJWTTokenExpireTime));


            //غیر فعالسازی نشست های فعال در sql
            await DeactiveExpiredSessions((int)user.Id, cancellationToken);

            //ایجاد session
            var expireTime = adminSettings.UserSessionMaxTime;

            //زمان منقضی شدن توکن اصلی که برای ست کردن تایمر رابط کاربری ارسال میشود
            var sessionExpireTime = adminSettings.UserSessionMaxTime;

            var authenticatedResponse = new AuthenticatedResponse_VM
            {
                Token = token.Token,
                RefreshToken = refreshToken,
                Name = user.Name,
                LastName = user.LastName,
                SessionExpireTime = sessionExpireTime,
                UserTypeId = user.Type,
                UserTypeDesc = user.Type == null ? "" : user.Type.GetDescription()


            };

            return new BaseResult_VM<AuthenticatedResponse_VM>(authenticatedResponse, 0, "لاگین با موفقیت انجام شد");
        }
        private async Task<Domain.Entities.Auth.User?> GetUser(string mobileNumber)
        {
            return await dbContext.User.Where(x => x.MobileNumber == mobileNumber).SingleOrDefaultAsync();
        }
        public async Task<int> GenerateActivationCode()
        {
            var random = new Random();
            int code = random.Next(1000, 9999);
            return code;
        }
        private async Task<List<Session_VM>> ExistingSession(int userId, IMapper mapper, CancellationToken cancellation)
        {
            var sessions = await dbContext.Session.Where(m => m.UserId == userId && m.Status == (int)SessionStatus.Active).ToListAsync();

            //نشست های فعال منقضی شده
            var expiredSessions = sessions.Where(m => m.ExpiresAt < DateTime.Now).ToList();

            //غیر فعالسازی نشست های فعال منقضی شده
            var result = await DeactiveExpiredSessions(userId, cancellation);

            //لیست نشست های فعال کاربر
            var activeSessionsQuery = sessions.Where(m => m.ExpiresAt > DateTime.Now);
            var activeSessions = _mapper.Map<List<Session_VM>>(activeSessionsQuery);

            if (!result)
                throw new Exception("کاربر گرامی در ذخیره سازی طلاعات نشست شما خطایی پیش آمده است. لطفا با مدیر سیستم تماس بگیرید");

            return activeSessions;
        }
        private async Task<bool> DeactiveExpiredSessions(int userId, CancellationToken cancellation)
        {
            var sessions = await dbContext.Session.Where(m => m.UserId == userId && m.Status == (int)SessionStatus.Active).FirstOrDefaultAsync();
            if (sessions != null)
            {


                sessions.Status = (int)SessionStatus.Inactive;
                dbContext.Session.Update(sessions);
                if (!(await dbContext.SaveChangesAsync(cancellation) > 0))
                    return false;
            }
            return true;
        }


        public class SMS_InputModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Body { get; set; }
        }

    }
}
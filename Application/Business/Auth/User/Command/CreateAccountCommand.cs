using Application.Business.Auth.User.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.Extentions;
using Application.Common.Interfaces.Auth;
using Application.Common.InterFaces.DbContext;
using Application.Common.Methodes;
using Domain.Enums.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.Auth.User.Command
{
    public class CreateAccountCommand : CreateUser_VM, IRequest<BaseResult_VM<string>>
    {
    }
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, BaseResult_VM<string>>
    {
        private readonly ITokenService _tokenService;
        private readonly IAdminDbContext _dbContext;
        private readonly IPasswordService _passwordService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;
        private readonly IShahkarValidation _shahkar;

        public CreateAccountCommandHandler(ITokenService tokenService, IAdminDbContext dbContext, IPasswordService passwordService,
            //IShahkar_SR Shahkar, ISabtAhval_SR sabtAhval,
            IHttpContextAccessor httpContextAccessor, IMediator mediator, IShahkarValidation shahkar)
        {
            _tokenService = tokenService;
            _dbContext = dbContext;
            _passwordService = passwordService;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
            _shahkar = shahkar;
        }
        public async Task<BaseResult_VM<string>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {

         

            //بررسی وجود کاربر 
            var userExist = await CheckExistUsername(request.MobileNumber);
            if (userExist) throw new Exception("شماره تماس شما در سیستم وجود دارد");

            //بررسی قوانین پسورد
            if (CheckPasswordPolicy.CheckPassworSecurity(request.Password) <= 3) throw new Exception("رمز عبور وارد شده معتبر نمیباشد لطفا رمز عبور دیگری استفاده نمایید");

            if(_dbContext.User.Any(x=>x.MobileNumber==request.MobileNumber)) throw new Exception("شماره موبایل وارد شده وجود دارد لطفا از شماره دیگری استفاده فرمایید  یا از قسمت ورود استفاده فرمایید ");

            //فراخوانی سرویس شاهکار 
            var shahkarResult = await _shahkar.Validation(request.MobileNumber, request.NationalCode);
            if (!shahkarResult)
            {
                return new BaseResult_VM<string>("", -2, "عدم تطابق شماره موبایل و کد ملی");
            }
            var userinformation = await _shahkar.NationalIdentityInquiry(request.NationalCode, request.BirthDate);
            if (userinformation == null||userinformation.Code!=0) throw new Exception("سرویس دریافت اطلاعات هویتی پاسخگو نیست ");

            //مقدار دهی  یوزر جدید 
            var user = new Domain.Entities.Auth.User
            {
                Username = request.MobileNumber,
                NationalCode = request.NationalCode,
                Password = request.Password,
                Name = userinformation.Result.FirstName.Trim().ConvertStandardPersian(),
                BirthDate = request.BirthDate,
                LastName = userinformation.Result.LastName.Trim().ConvertStandardPersian(),
                MobileNumber = request.MobileNumber,
                Status = (int)UserStatus.PendingAcceptAdmin
            };

            if (request.Password.Trim() != request.ConfirmPassword.Trim()) throw new Exception("رمز عبور و تکرار آن یکسان  نمیباشد");

            //ایجاد پسورد کاربر 
            byte[] salt = null;
            var password_VM = _passwordService.HashPassword(user.Password, salt);
            user.Salt = password_VM.Salt;
            user.Password = password_VM.HashedPassword;

            if (await CreateAccount(user, cancellationToken) == false) throw new Exception();

            return new BaseResult_VM<string>("", 0, "کاربر با موفقیت  اضافه شد");

        }

        //public async Task<BaseResult_VM<string>> SabtAhvalInquery(SabtAhval inputModel, CreateAccountCommand request)
        //{
        //    var methodResult = await _sRCaller.CallSR(new Domain.Dto.SRRequest
        //    {
        //        CallingMode = ServiceCallingMode.Immediate,
        //        Input = inputModel,
        //        MethodName = MethodName.SabtAhval_GetPersonInfoOnline,
        //        SendTimeoutSecounds = 60,
        //        PointerId = Convert.ToInt64(request.Username),
        //    }, TimeSpan.FromSeconds(61));

        //    if (!methodResult.Success)
        //    {
        //        throw new BadResponseServiceException();
        //    }
        //    if (methodResult.Code != 0)
        //    {
        //        return new BaseResult_VM<string>(null, methodResult.Code, methodResult.Message);
        //    }
        //    if (methodResult.Result.Success != SuccessInfo.Success)
        //    {
        //        return new BaseResult_VM<string>(null, methodResult.Code, "کاربر گرامی پاسخ مناسبی از سرویس دریافت نگردید");
        //    }
        //    Estelam3Result_VM sabtAhvalResult = JsonConvert.DeserializeObject<Estelam3Result_VM>(methodResult.Result.Response);

        //    // تطابق نام و نام خانوادگی وارد شده با ثبت احوال
        //    if (sabtAhvalResult.getEstelam3Result[0].Name.Trim().ConvertStandardPersian() != request.Name.ConvertStandardPersian())
        //        return new BaseResult_VM<string>("Name", -200, "کاربر گرامی نام وارد شده با کد ملی تطابق ندارد");

        //    if (sabtAhvalResult.getEstelam3Result[0].Family.Trim().ConvertStandardPersian() != request.LastName.ConvertStandardPersian())
        //        return new BaseResult_VM<string>("LastName", -200, "کاربر گرامی نام خانوادگی وارد شده با کد ملی تطابق ندارد");

        //    return new BaseResult_VM<string>(null, 0, "ادامه فرآیند مشکلی ندارد");
        //}

        private async Task<bool> CreateAccount(Domain.Entities.Auth.User user, CancellationToken cancellationToken)
        {
            await _dbContext.User.AddAsync(user);
            return await _dbContext.SaveChangesAsync(cancellationToken) != 0;
        }

        public async Task<bool> CheckExistUsername(string username)
        {
            var user = await _dbContext.User.Where(x => x.Username == username).FirstOrDefaultAsync();
            return user != null;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Business.Auth.User.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.DbContext;
using MediatR;

namespace Application.Business.Auth.User.Command
{
    public class UpdateAdminUserCommand : IRequest<BaseResult_VM<bool>>
    {
        public UpdateUser_VM Input { get; set; }
    }
    public class UpdateUserCommandHandler : IRequestHandler<UpdateAdminUserCommand, BaseResult_VM<bool>>
    {
        private readonly IAdminDbContext _dbContext;

        public UpdateUserCommandHandler(IAdminDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<BaseResult_VM<bool>> Handle(UpdateAdminUserCommand request, CancellationToken cancellationToken)
        {
            var result = new BaseResult_VM<bool>();

            var user = await _dbContext.User.FindAsync(new object[] { request.Input.Id }, cancellationToken);

            if (user == null)
            {
                result.Result = false;
                result.Message = "کاربر یافت نشد.";
                return result;
            }

            // فقط فیلدهایی که مقدار دارند، بروزرسانی می‌شوند
            if (!string.IsNullOrWhiteSpace(request.Input.Name))
                user.Name = request.Input.Name;

            if (!string.IsNullOrWhiteSpace(request.Input.LastName))
                user.LastName = request.Input.LastName;

            if (!string.IsNullOrWhiteSpace(request.Input.CompanyName))
                user.CompanyName = request.Input.CompanyName;

            if (!string.IsNullOrWhiteSpace(request.Input.MobileNumber))
                user.MobileNumber = request.Input.MobileNumber;

            if (!string.IsNullOrWhiteSpace(request.Input.NationalCode))
                user.NationalCode = request.Input.NationalCode;

            if (!string.IsNullOrWhiteSpace(request.Input.BirthDate))
                user.BirthDate = request.Input.BirthDate;

            if (request.Input.Status!=null)
                user.Status = request.Input.Status;

            if (request.Input.TwoStageLogin!=null)
                user.TwoStageLogin = request.Input.TwoStageLogin;

            if (request.Input.Type!=null)
                user.Type = request.Input.Type;

            if (request.Input.UserRate!=null)
                user.UserRate = request.Input.UserRate;

            // اگر رمز جدید ارسال شده
            if (!string.IsNullOrWhiteSpace(request.Input.Password))
            {
                // ساخت salt
                using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
                {
                    var salt = new byte[16];
                    rng.GetBytes(salt);
                    user.Salt = salt;
                }

                // هش رمز عبور
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    var combined = Encoding.UTF8.GetBytes(request.Input.Password).Concat(user.Salt).ToArray();
                    var hash = sha256.ComputeHash(combined);
                    user.Password = Convert.ToBase64String(hash);
                }
            }

            _dbContext.User.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            result.Result = true;
            result.Message = "کاربر با موفقیت بروزرسانی شد.";
            return result;
        }
    }
}

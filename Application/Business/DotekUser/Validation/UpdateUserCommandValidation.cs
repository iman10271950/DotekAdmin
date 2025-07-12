using Application.Business.DotekUser.Command;
using Application.Business.DotekUser.VieModel;
using FluentValidation;

namespace Application.Business.DotekUser.Validation;

public class UpdateUserCommandValidation:AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidation()
    {
        // اعتبارسنجی فیلد User
        RuleFor(x => x.User)
            .NotNull().WithMessage("فیلد User نمی‌تواند خالی باشد.")
            .SetValidator(new DotekUser_VMValidator());
    }
}
public class DotekUser_VMValidator : AbstractValidator<DotekUser_VM>
{
    public DotekUser_VMValidator()
    {
        // اعتبارسنجی فیلد Id
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id باید یک عدد مثبت باشد.");

        // اعتبارسنجی فیلد Name
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("نام نمی‌تواند خالی باشد.")
            .Length(3, 50).WithMessage("نام باید بین 3 تا 50 کاراکتر باشد.");

        // اعتبارسنجی فیلد LastName
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("نام خانوادگی نمی‌تواند خالی باشد.")
            .Length(3, 50).WithMessage("نام خانوادگی باید بین 3 تا 50 کاراکتر باشد.");

      

        // اعتبارسنجی فیلد MobileNumber
        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage("شماره موبایل نمی‌تواند خالی باشد.")
            .Matches(@"^(\+98|0)?9\d{9}$").WithMessage("شماره موبایل باید با فرمت معتبر ایران وارد شود.");

        // اعتبارسنجی فیلد NationalCode
        RuleFor(x => x.NationalCode)
            .NotEmpty().WithMessage("کد ملی نمی‌تواند خالی باشد.")
            .Matches(@"^\d{10}$").WithMessage("کد ملی باید 10 رقم باشد.");

        // اعتبارسنجی فیلد BirthDate
        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("تاریخ تولد نمی‌تواند خالی باشد.")
            .Matches(@"^\d{4}/\d{2}/\d{2}$").WithMessage("تاریخ تولد باید به فرمت YYYY/MM/DD باشد.");

        // اعتبارسنجی فیلد Status
        RuleFor(x => x.Status)
            .InclusiveBetween(1, 10).WithMessage("وضعیت باید بین 1 تا 10 باشد.");

        // اعتبارسنجی فیلد Type
        RuleFor(x => x.Type)
            .InclusiveBetween(1, 5).WithMessage("نوع باید بین 1 تا 5 باشد.");

 

        
    }
}
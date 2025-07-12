using Application.Business.DotekRequest.Query;
using FluentValidation;

namespace Application.Business.DotekRequest.Validation;

public class UpdateDotekRequestStatusCommandValidation:AbstractValidator<UpdateDotekRequestStatusCommand>
{
    public UpdateDotekRequestStatusCommandValidation()
    {
        // اعتبارسنجی فیلد RequestIdList
        RuleFor(x => x.RequestIdList)
            .NotNull().WithMessage("فیلد RequestIdList نمی‌تواند خالی باشد.")
            .NotEmpty().WithMessage("فیلد RequestIdList باید حداقل یک آیتم داشته باشد.");

        // اعتبارسنجی فیلد Requeststatus
        RuleFor(x => x.Requeststatus)
            .GreaterThanOrEqualTo(0).WithMessage("Requeststatus باید یک عدد مثبت باشد.")
            .LessThanOrEqualTo(9).WithMessage("Requeststatus باید کمتر از یا مساوی 9 باشد.");
    }
}
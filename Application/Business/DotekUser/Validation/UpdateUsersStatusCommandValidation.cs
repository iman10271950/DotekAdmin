using Application.Business.DotekUser.Command;
using FluentValidation;

namespace Application.Business.DotekUser.Validation;

public class UpdateUsersStatusCommandValidation:AbstractValidator<UpdateUsersStatusCommand>
{
    public UpdateUsersStatusCommandValidation()
    {
        // اعتبارسنجی فیلد UserIdList
        RuleFor(x => x.UserIdList)
            .NotNull().WithMessage("فیلد UserIdList نمی‌تواند خالی باشد.")
            .Must(list => list != null && list.Count > 0).WithMessage("فیلد UserIdList باید حداقل یک آیتم داشته باشد.");

        // اعتبارسنجی فیلد Status
        RuleFor(x => x.Status)
            .InclusiveBetween(1, 10).WithMessage("وضعیت باید بین 1 تا 10 باشد.");
    }
}
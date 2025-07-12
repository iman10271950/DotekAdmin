using Application.Business.DotekUser.Command;
using FluentValidation;

namespace Application.Business.DotekUser.Validation;

public class DeleteDotekRolleCommandValidation:AbstractValidator<DeleteDotekRolleCommand>
{
    public DeleteDotekRolleCommandValidation()
    {
        // اعتبارسنجی فیلد Role
        RuleFor(x => x.Role)
            .NotNull().WithMessage("فیلد Role نمی‌تواند خالی باشد.");

    }
}
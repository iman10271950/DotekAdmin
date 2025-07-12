using Application.Business.DotekUser.Command;
using FluentValidation;

namespace Application.Business.DotekUser.Validation;

public class InactiveDotekUserCommandValidation:AbstractValidator<InactiveDotekUserCommand>
{
    public InactiveDotekUserCommandValidation()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage("فیلد UserId نمی‌تواند خالی باشد.");
    }
}
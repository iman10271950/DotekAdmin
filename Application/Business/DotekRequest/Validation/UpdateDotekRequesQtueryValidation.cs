using Application.Business.DotekRequest.Query;
using FluentValidation;

namespace Application.Business.DotekRequest.Validation;

public class UpdateDotekRequesQtueryValidation:AbstractValidator<UpdateDotekRequesQtuery>
{
    public UpdateDotekRequesQtueryValidation()
    {

        RuleFor(x => x.Input)
            .NotNull().WithMessage("فیلد Input نمی‌تواند خالی باشد.");

        // اعتبارسنجی فیلد OtherProperty
        RuleFor(x => x.OtherProperty)
            .NotNull().WithMessage("فیلد OtherProperty نمی‌تواند خالی باشد.")
            .Must(list => list != null && list.Count > 0).WithMessage("فیلد OtherProperty باید حداقل یک آیتم داشته باشد.");
    }
}
using Client.MVC.Models.Discounts;
using FluentValidation;

namespace Client.MVC.Validators;

public class DiscountApplyInputValidator : AbstractValidator<DiscountApplyInput>
{
    public DiscountApplyInputValidator()
    {
        RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Code is required!");
    }
}

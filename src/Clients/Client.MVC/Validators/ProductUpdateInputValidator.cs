using Client.MVC.Models.Catalog;
using FluentValidation;

namespace Client.MVC.Validators;

public class ProductUpdateInputValidator : AbstractValidator<ProductUpdateInput>
{
    public ProductUpdateInputValidator()
    {
        RuleFor(x => x.Name)
        .NotEmpty()
        .WithMessage("Name is required!");

        RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required!");

        RuleFor(x => x.Count)
                .NotEmpty()
                .WithMessage("Count is required!")
                .GreaterThan(0)
                .WithMessage("Count must be grater than zero!");

        RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage("Price is required!")
                .ScalePrecision(2, 6)
                .WithMessage("Wrong money format!");
    }
}

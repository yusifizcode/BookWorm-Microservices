using Client.MVC.Models.Catalog;
using FluentValidation;

namespace Client.MVC.Validators;

public class CourseUpdateInputValidator : AbstractValidator<CourseUpdateInput>
{
    public CourseUpdateInputValidator()
    {
        RuleFor(x => x.Name)
        .NotEmpty()
        .WithMessage("Name is required!");

        RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required!");

        RuleFor(x => x.Feature.Duration)
                .InclusiveBetween(1, int.MaxValue)
                .WithMessage("Duration is required!");

        RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage("Price is required!")
                .ScalePrecision(2, 6)
                .WithMessage("Wrong money format!");
    }
}

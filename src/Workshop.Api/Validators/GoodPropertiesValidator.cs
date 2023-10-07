using FluentValidation;
using Workshop.Api.Requests.V3;

namespace Workshop.Api.Validators;

public class GoodPropertiesValidator : AbstractValidator<GoodProperties>
{
    public GoodPropertiesValidator()
    {
        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .LessThan(int.MaxValue);
        RuleFor(x => x.Length)
            .GreaterThan(0)
            .LessThan(int.MaxValue);
        RuleFor(x => x.Width)
            .GreaterThan(0)
            .LessThan(int.MaxValue);
        RuleFor(x => x.Height)
            .GreaterThan(0)
            .LessThan(int.MaxValue);
    }
}
using FluentValidation;
using Workshop.Api.Requests.V3;

namespace Workshop.Api.Validators;

public class CalculatorRequestValidator : AbstractValidator<CalculateRequest>
{
    public CalculatorRequestValidator()
    {
        RuleForEach(x => x.Goods)
            .SetValidator(new GoodPropertiesValidator());

    }
}
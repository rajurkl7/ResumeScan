using FluentValidation;

namespace ResumeScanApi.Application.Cities;

public sealed class CreateCityRequestValidator : AbstractValidator<CreateCityRequest>
{
    public CreateCityRequestValidator()
    {
        RuleFor(x => x.CityId).GreaterThan(0);
        RuleFor(x => x.StateId).GreaterThan(0);
        RuleFor(x => x.CityName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CreatedDateTime).NotEmpty();
    }
}

public sealed class UpdateCityRequestValidator : AbstractValidator<UpdateCityRequest>
{
    public UpdateCityRequestValidator()
    {
        RuleFor(x => x.StateId).GreaterThan(0);
        RuleFor(x => x.CityName).NotEmpty().MaximumLength(100);
    }
}

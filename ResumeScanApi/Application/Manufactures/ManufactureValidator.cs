using FluentValidation;

namespace ResumeScanApi.Application.Manufactures;

public sealed class CreateManufactureRequestValidator : AbstractValidator<CreateManufactureRequest>
{
    public CreateManufactureRequestValidator()
    {
        RuleFor(x => x.ManufactureId).GreaterThan(0);
        RuleFor(x => x.ManufactureName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CreatedDateTime).NotEmpty();
    }
}

public sealed class UpdateManufactureRequestValidator : AbstractValidator<UpdateManufactureRequest>
{
    public UpdateManufactureRequestValidator()
    {
        RuleFor(x => x.ManufactureName).NotEmpty().MaximumLength(50);
    }
}

using FluentValidation;

namespace ResumeScanApi.Application.EquipmentModalities;

public sealed class CreateEquipmentModalityRequestValidator : AbstractValidator<CreateEquipmentModalityRequest>
{
    public CreateEquipmentModalityRequestValidator()
    {
        RuleFor(x => x.EquipmentModalityId).GreaterThan(0);
        RuleFor(x => x.EquipmentModalityName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CreatedDateTime).NotEmpty();
    }
}

public sealed class UpdateEquipmentModalityRequestValidator : AbstractValidator<UpdateEquipmentModalityRequest>
{
    public UpdateEquipmentModalityRequestValidator()
    {
        RuleFor(x => x.EquipmentModalityName).NotEmpty().MaximumLength(100);
    }
}

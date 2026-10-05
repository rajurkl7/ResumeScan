using FluentValidation;

namespace ResumeScanApi.Application.CustomerEquipmentRegistrations;

public sealed class CreateCustomerEquipmentRegistrationRequestValidator : AbstractValidator<CreateCustomerEquipmentRegistrationRequest>
{
    public CreateCustomerEquipmentRegistrationRequestValidator()
    {
        RuleFor(x => x.FacilityName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CustomerContactName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CustomerContactMobile).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PrimaryContactEmail).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.FacilityAddress).NotEmpty().MaximumLength(500);
        RuleFor(x => x.EquipmentModalityId).GreaterThan(0);
        RuleFor(x => x.ManufactureId).GreaterThan(0);
        RuleFor(x => x.ModelIdentifier).NotEmpty().MaximumLength(100);
        RuleFor(x => x.EquipmentSerialNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.SoftwareFirmwareVersion).MaximumLength(50);
        RuleFor(x => x.CoverageStatusId).GreaterThan(0);
        RuleFor(x => x.CreatedDateTime).NotEmpty();
    }
}

public sealed class UpdateCustomerEquipmentRegistrationRequestValidator : AbstractValidator<UpdateCustomerEquipmentRegistrationRequest>
{
    public UpdateCustomerEquipmentRegistrationRequestValidator()
    {
        RuleFor(x => x.FacilityName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CustomerContactName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CustomerContactMobile).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PrimaryContactEmail).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.FacilityAddress).NotEmpty().MaximumLength(500);
        RuleFor(x => x.EquipmentModalityId).GreaterThan(0);
        RuleFor(x => x.ManufactureId).GreaterThan(0);
        RuleFor(x => x.ModelIdentifier).NotEmpty().MaximumLength(100);
        RuleFor(x => x.EquipmentSerialNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.SoftwareFirmwareVersion).MaximumLength(50);
        RuleFor(x => x.CoverageStatusId).GreaterThan(0);
    }
}

using FluentValidation;

namespace ResumeScanApi.Application.UserTypes;

public sealed class CreateUserTypeRequestValidator : AbstractValidator<CreateUserTypeRequest>
{
    public CreateUserTypeRequestValidator()
    {
        RuleFor(x => x.UserTypeId).GreaterThan(0);
        RuleFor(x => x.UserType).NotEmpty().MaximumLength(50);
    }
}

public sealed class UpdateUserTypeRequestValidator : AbstractValidator<UpdateUserTypeRequest>
{
    public UpdateUserTypeRequestValidator()
    {
        RuleFor(x => x.UserType).NotEmpty().MaximumLength(50);
    }
}

using FluentValidation;

namespace ResumeScanApi.Application.States;

public sealed class CreateStateRequestValidator : AbstractValidator<CreateStateRequest>
{
    public CreateStateRequestValidator()
    {
        RuleFor(x => x.StateId).GreaterThan(0);
        RuleFor(x => x.StateName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CreatedDateTime).NotEmpty();
    }
}

public sealed class UpdateStateRequestValidator : AbstractValidator<UpdateStateRequest>
{
    public UpdateStateRequestValidator()
    {
        RuleFor(x => x.StateName).NotEmpty().MaximumLength(100);
    }
}

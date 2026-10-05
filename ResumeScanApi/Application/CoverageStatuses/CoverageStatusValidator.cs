using FluentValidation;

namespace ResumeScanApi.Application.CoverageStatuses;

public sealed class CreateCoverageStatusRequestValidator : AbstractValidator<CreateCoverageStatusRequest>
{
    public CreateCoverageStatusRequestValidator()
    {
        RuleFor(x => x.CoverageStatusId).GreaterThan(0);
        RuleFor(x => x.CoverageStatusName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CreatedDateTime).NotEmpty();
    }
}

public sealed class UpdateCoverageStatusRequestValidator : AbstractValidator<UpdateCoverageStatusRequest>
{
    public UpdateCoverageStatusRequestValidator()
    {
        RuleFor(x => x.CoverageStatusName).NotEmpty().MaximumLength(100);
    }
}

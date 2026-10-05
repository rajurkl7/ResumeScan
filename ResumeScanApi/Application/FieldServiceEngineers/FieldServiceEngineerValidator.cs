using FluentValidation;

namespace ResumeScanApi.Application.FieldServiceEngineers;

public sealed class CreateFieldServiceEngineerRequestValidator : AbstractValidator<CreateFieldServiceEngineerRequest>
{
    public CreateFieldServiceEngineerRequestValidator()
    {
        RuleFor(x => x.Documents).NotEmpty();
        RuleFor(x => x.EngineerName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.BaseLocation).NotEmpty().MaximumLength(200);
        RuleFor(x => x.MobileNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.EmploymentStatus).Must(IsValidEmploymentStatus).WithMessage("EmploymentStatus must be Full-Time, Part-Time, or Freelancer.");
        RuleFor(x => x.Capabilities).NotEmpty();
        RuleForEach(x => x.Capabilities).SetValidator(new EngineerCapabilityRequestValidator());
        RuleFor(x => x.OperatingStates).NotEmpty();
        RuleForEach(x => x.OperatingStates).SetValidator(new EngineerStateRequestValidator());
        RuleFor(x => x.OperatingCities).NotEmpty();
        RuleForEach(x => x.OperatingCities).SetValidator(new EngineerCityRequestValidator());
        RuleForEach(x => x.Documents).SetValidator(new EngineerDocumentRequestValidator());
        RuleForEach(x => x.Documents)
            .Must(document => EngineerDocumentPayloadValidator.HasValidPayload(document.FileContentBase64))
            .WithMessage("Each document must include a valid file upload of 10 MB or less.");
        RuleFor(x => x.Documents)
            .Must(EngineerDocumentPayloadValidator.AreWithinTotalSizeLimit)
            .WithMessage("The total size of uploaded documents must be 20 MB or less.");
        RuleFor(x => x.CreatedDateTime).NotEmpty();
    }

    private static bool IsValidEmploymentStatus(string value) => value is "Full-Time" or "Part-Time" or "Freelancer";
}

public sealed class UpdateFieldServiceEngineerRequestValidator : AbstractValidator<UpdateFieldServiceEngineerRequest>
{
    public UpdateFieldServiceEngineerRequestValidator()
    {
        RuleFor(x => x.EngineerName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.BaseLocation).NotEmpty().MaximumLength(200);
        RuleFor(x => x.MobileNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.EmploymentStatus).Must(value => value is "Full-Time" or "Part-Time" or "Freelancer")
            .WithMessage("EmploymentStatus must be Full-Time, Part-Time, or Freelancer.");
        RuleFor(x => x.Capabilities).NotEmpty();
        RuleForEach(x => x.Capabilities).SetValidator(new EngineerCapabilityRequestValidator());
        RuleFor(x => x.OperatingStates).NotEmpty();
        RuleForEach(x => x.OperatingStates).SetValidator(new EngineerStateRequestValidator());
        RuleFor(x => x.OperatingCities).NotEmpty();
        RuleForEach(x => x.OperatingCities).SetValidator(new EngineerCityRequestValidator());
        RuleForEach(x => x.Documents).SetValidator(new EngineerDocumentRequestValidator());
    }
}

public sealed class EngineerCapabilityRequestValidator : AbstractValidator<EngineerCapabilityRequest>
{
    public EngineerCapabilityRequestValidator()
    {
        RuleFor(x => x.EquipmentModalityId).GreaterThan(0);
        RuleFor(x => x.ManufactureId).GreaterThan(0);
    }
}

public sealed class EngineerStateRequestValidator : AbstractValidator<EngineerStateRequest>
{
    public EngineerStateRequestValidator()
    {
        RuleFor(x => x.StateId).GreaterThan(0);
    }
}

public sealed class EngineerCityRequestValidator : AbstractValidator<EngineerCityRequest>
{
    public EngineerCityRequestValidator()
    {
        RuleFor(x => x.CityId).GreaterThan(0);
    }
}

public sealed class EngineerDocumentRequestValidator : AbstractValidator<EngineerDocumentRequest>
{
    public EngineerDocumentRequestValidator()
    {
        RuleFor(x => x.DocumentType).Must(value => value is "Training" or "Experience")
            .WithMessage("DocumentType must be Training or Experience.");
        RuleFor(x => x.OriginalFileName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.StoredFilePath).NotEmpty().MaximumLength(1000)
            .When(x => string.IsNullOrWhiteSpace(x.FileContentBase64));
        RuleFor(x => x.FileSizeBytes).GreaterThan(0).When(x => x.FileSizeBytes.HasValue);
        RuleFor(x => x.OriginalFileName)
            .Must(EngineerDocumentPayloadValidator.HasSupportedExtension)
            .WithMessage("Documents must be PDF, JPG, JPEG, or PNG files.");
    }
}

public static class EngineerDocumentPayloadValidator
{
    public const int MaxDocumentSizeBytes = 10 * 1024 * 1024;
    public const int MaxTotalDocumentSizeBytes = 20 * 1024 * 1024;

    public static bool HasValidPayload(string? payload)
    {
        if (string.IsNullOrWhiteSpace(payload)
            || payload.Length > GetMaxBase64Length(MaxDocumentSizeBytes))
        {
            return false;
        }

        try
        {
            return Convert.FromBase64String(payload).Length is > 0 and <= MaxDocumentSizeBytes;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public static bool AreWithinTotalSizeLimit(IReadOnlyList<EngineerDocumentRequest>? documents)
    {
        if (documents is null || documents.Count == 0)
        {
            return false;
        }

        long totalSize = 0;
        foreach (var document in documents)
        {
            if (string.IsNullOrWhiteSpace(document.FileContentBase64)
                || document.FileContentBase64.Length > GetMaxBase64Length(MaxTotalDocumentSizeBytes))
            {
                return false;
            }

            try
            {
                totalSize += Convert.FromBase64String(document.FileContentBase64).Length;
            }
            catch (FormatException)
            {
                return false;
            }

            if (totalSize > MaxTotalDocumentSizeBytes)
            {
                return false;
            }
        }

        return true;
    }

    private static int GetMaxBase64Length(int byteCount) => 4 * ((byteCount + 2) / 3);

    public static bool HasSupportedExtension(string fileName)
        => !string.IsNullOrWhiteSpace(fileName)
            && Path.GetExtension(fileName).ToLowerInvariant() is ".pdf" or ".jpg" or ".jpeg" or ".png";
}

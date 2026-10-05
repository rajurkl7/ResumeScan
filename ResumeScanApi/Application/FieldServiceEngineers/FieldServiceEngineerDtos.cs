namespace ResumeScanApi.Application.FieldServiceEngineers;

public sealed class FieldServiceEngineerDto
{
    public int FieldServiceEngineerId { get; set; }
    public string EngineerName { get; set; } = string.Empty;
    public string BaseLocation { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string EmploymentStatus { get; set; } = string.Empty;
    public IReadOnlyList<EngineerCapabilityDto> Capabilities { get; set; } = [];
    public IReadOnlyList<EngineerStateDto> OperatingStates { get; set; } = [];
    public IReadOnlyList<EngineerCityDto> OperatingCities { get; set; } = [];
    public IReadOnlyList<EngineerDocumentDto> Documents { get; set; } = [];
    public bool IsEnabled { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime ModifiedDateTime { get; set; }
}

public sealed record CreateFieldServiceEngineerRequest(
    string EngineerName,
    string BaseLocation,
    string MobileNumber,
    string EmailAddress,
    string Password,
    string EmploymentStatus,
    IReadOnlyList<EngineerCapabilityRequest> Capabilities,
    IReadOnlyList<EngineerStateRequest> OperatingStates,
    IReadOnlyList<EngineerCityRequest> OperatingCities,
    IReadOnlyList<EngineerDocumentRequest> Documents,
    bool IsEnabled,
    DateTime CreatedDateTime);

public sealed record UpdateFieldServiceEngineerRequest(
    string EngineerName,
    string BaseLocation,
    string MobileNumber,
    string EmailAddress,
    string Password,
    string EmploymentStatus,
    IReadOnlyList<EngineerCapabilityRequest> Capabilities,
    IReadOnlyList<EngineerStateRequest> OperatingStates,
    IReadOnlyList<EngineerCityRequest> OperatingCities,
    IReadOnlyList<EngineerDocumentRequest> Documents,
    bool IsEnabled);

public sealed record EngineerLoginRequest(string Email, string Password);

public sealed record EngineerCapabilityDto(int EquipmentModalityId, int ManufactureId);
public sealed record EngineerStateDto(int StateId, string StateName);
public sealed record EngineerCityDto(int CityId, int StateId, string CityName, string StateName);
public sealed record EngineerDocumentDto(int FieldServiceEngineerDocumentId, string DocumentType, string OriginalFileName, string StoredFilePath, string? ContentType, long? FileSizeBytes, DateTime UploadedDateTime);
public sealed record EngineerCapabilityRequest(int EquipmentModalityId, int ManufactureId);
public sealed record EngineerStateRequest(int StateId);
public sealed record EngineerCityRequest(int CityId);
public sealed record EngineerDocumentRequest(string DocumentType, string OriginalFileName, string StoredFilePath, string? ContentType, long? FileSizeBytes, string? FileContentBase64 = null);

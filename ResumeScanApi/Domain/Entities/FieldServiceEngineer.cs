namespace ResumeScanApi.Domain.Entities;

public sealed class FieldServiceEngineer
{
    public int FieldServiceEngineerId { get; set; }
    public string EngineerName { get; set; } = string.Empty;
    public string BaseLocation { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string EmploymentStatus { get; set; } = string.Empty;
    public List<FieldServiceEngineerCapability> Capabilities { get; set; } = [];
    public List<FieldServiceEngineerState> OperatingStates { get; set; } = [];
    public List<FieldServiceEngineerCity> OperatingCities { get; set; } = [];
    public List<FieldServiceEngineerDocument> Documents { get; set; } = [];
    public bool IsEnabled { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime ModifiedDateTime { get; set; }
    public string? CapabilitiesJson { get; set; }
    public string? StatesJson { get; set; }
    public string? CitiesJson { get; set; }
    public string? DocumentsJson { get; set; }
}

public sealed class FieldServiceEngineerCapability
{
    public int EquipmentModalityId { get; set; }
    public int ManufactureId { get; set; }
}

public sealed class FieldServiceEngineerState
{
    public int StateId { get; set; }
    public string StateName { get; set; } = string.Empty;
}

public sealed class FieldServiceEngineerCity
{
    public int CityId { get; set; }
    public int StateId { get; set; }
    public string CityName { get; set; } = string.Empty;
    public string StateName { get; set; } = string.Empty;
}

public sealed class FieldServiceEngineerDocument
{
    public int FieldServiceEngineerDocumentId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string StoredFilePath { get; set; } = string.Empty;
    public string? ContentType { get; set; }
    public long? FileSizeBytes { get; set; }
    public DateTime UploadedDateTime { get; set; }
}

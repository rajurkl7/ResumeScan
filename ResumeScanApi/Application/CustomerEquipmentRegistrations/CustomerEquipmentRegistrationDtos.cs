namespace ResumeScanApi.Application.CustomerEquipmentRegistrations;

public sealed class CustomerEquipmentRegistrationDto
{
    public int CustomerEquipmentRegistrationId { get; set; }
    public string FacilityName { get; set; } = string.Empty;
    public string CustomerContactName { get; set; } = string.Empty;
    public string CustomerContactMobile { get; set; } = string.Empty;
    public string PrimaryContactEmail { get; set; } = string.Empty;
    public string FacilityAddress { get; set; } = string.Empty;
    public int EquipmentModalityId { get; set; }
    public int ManufactureId { get; set; }
    public string ModelIdentifier { get; set; } = string.Empty;
    public string EquipmentSerialNumber { get; set; } = string.Empty;
    public string? SoftwareFirmwareVersion { get; set; }
    public int CoverageStatusId { get; set; }
    public bool IsEnabled { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime ModifiedDateTime { get; set; }
}

public sealed record CreateCustomerEquipmentRegistrationRequest(
    string FacilityName,
    string CustomerContactName,
    string CustomerContactMobile,
    string PrimaryContactEmail,
    string Password,
    string FacilityAddress,
    int EquipmentModalityId,
    int ManufactureId,
    string ModelIdentifier,
    string EquipmentSerialNumber,
    string? SoftwareFirmwareVersion,
    int CoverageStatusId,
    bool IsEnabled,
    DateTime CreatedDateTime);

public sealed record UpdateCustomerEquipmentRegistrationRequest(
    string FacilityName,
    string CustomerContactName,
    string CustomerContactMobile,
    string PrimaryContactEmail,
    string Password,
    string FacilityAddress,
    int EquipmentModalityId,
    int ManufactureId,
    string ModelIdentifier,
    string EquipmentSerialNumber,
    string? SoftwareFirmwareVersion,
    int CoverageStatusId,
    bool IsEnabled);

public sealed record CustomerLoginRequest(string Email, string Password);

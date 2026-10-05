namespace ResumeScanApi.Domain.Entities;

public sealed class CustomerEquipmentRegistration
{
    public int CustomerEquipmentRegistrationId { get; set; }
    public string FacilityName { get; set; } = string.Empty;
    public string CustomerContactName { get; set; } = string.Empty;
    public string CustomerContactMobile { get; set; } = string.Empty;
    public string PrimaryContactEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
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

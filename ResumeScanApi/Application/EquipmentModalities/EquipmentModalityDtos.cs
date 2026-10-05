namespace ResumeScanApi.Application.EquipmentModalities;

public sealed class EquipmentModalityDto
{
    public int EquipmentModalityId { get; set; }
    public string EquipmentModalityName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public DateTime CreatedDateTime { get; set; }
}

public sealed record CreateEquipmentModalityRequest(
    int EquipmentModalityId,
    string EquipmentModalityName,
    bool IsEnabled,
    DateTime CreatedDateTime);

public sealed record UpdateEquipmentModalityRequest(string EquipmentModalityName, bool IsEnabled);

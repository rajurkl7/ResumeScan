namespace ResumeScanApi.Domain.Entities;

public sealed class EquipmentModality
{
    public int EquipmentModalityId { get; set; }
    public string EquipmentModalityName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public DateTime CreatedDateTime { get; set; }
}

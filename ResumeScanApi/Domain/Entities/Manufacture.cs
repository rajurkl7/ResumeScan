namespace ResumeScanApi.Domain.Entities;

public sealed class Manufacture
{
    public int ManufactureId { get; set; }
    public string ManufactureName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public DateTime CreatedDateTime { get; set; }
}

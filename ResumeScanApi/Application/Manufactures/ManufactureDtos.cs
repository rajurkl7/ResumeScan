namespace ResumeScanApi.Application.Manufactures;

public sealed class ManufactureDto
{
    public int ManufactureId { get; set; }
    public string ManufactureName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public DateTime CreatedDateTime { get; set; }
}

public sealed record CreateManufactureRequest(
    int ManufactureId,
    string ManufactureName,
    bool IsEnabled,
    DateTime CreatedDateTime);

public sealed record UpdateManufactureRequest(string ManufactureName, bool IsEnabled);

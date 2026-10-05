namespace ResumeScanApi.Domain.Entities;

public sealed class City
{
    public int CityId { get; set; }
    public int StateId { get; set; }
    public string StateName { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public DateTime CreatedDateTime { get; set; }
}

namespace ResumeScanApi.Application.Cities;

public sealed class CityDto
{
    public int CityId { get; set; }
    public int StateId { get; set; }
    public string StateName { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public DateTime CreatedDateTime { get; set; }
}

public sealed record CreateCityRequest(int CityId, int StateId, string CityName, bool IsEnabled, DateTime CreatedDateTime);
public sealed record UpdateCityRequest(int StateId, string CityName, bool IsEnabled);

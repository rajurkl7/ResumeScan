namespace ResumeScanApi.Application.CoverageStatuses;

public sealed class CoverageStatusDto
{
    public int CoverageStatusId { get; set; }
    public string CoverageStatusName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public DateTime CreatedDateTime { get; set; }
}

public sealed record CreateCoverageStatusRequest(
    int CoverageStatusId,
    string CoverageStatusName,
    bool IsEnabled,
    DateTime CreatedDateTime);

public sealed record UpdateCoverageStatusRequest(string CoverageStatusName, bool IsEnabled);

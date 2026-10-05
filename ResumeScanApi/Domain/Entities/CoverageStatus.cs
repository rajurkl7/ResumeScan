namespace ResumeScanApi.Domain.Entities;

public sealed class CoverageStatus
{
    public int CoverageStatusId { get; set; }
    public string CoverageStatusName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public DateTime CreatedDateTime { get; set; }
}

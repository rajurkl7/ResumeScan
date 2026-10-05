namespace ResumeScanApi.Domain.Entities;

public sealed class State
{
    public int StateId { get; set; }
    public string StateName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public DateTime CreatedDateTime { get; set; }
}

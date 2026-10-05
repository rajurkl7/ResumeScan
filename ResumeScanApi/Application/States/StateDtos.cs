namespace ResumeScanApi.Application.States;

public sealed class StateDto
{
    public int StateId { get; set; }
    public string StateName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public DateTime CreatedDateTime { get; set; }
}

public sealed record CreateStateRequest(int StateId, string StateName, bool IsEnabled, DateTime CreatedDateTime);
public sealed record UpdateStateRequest(string StateName, bool IsEnabled);

namespace ResumeScanApi.Domain.Entities;

public sealed class UserType
{
    public int UserTypeId { get; set; }
    public string UserTypeName { get; set; } = string.Empty;
}

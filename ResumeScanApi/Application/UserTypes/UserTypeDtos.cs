namespace ResumeScanApi.Application.UserTypes;

public sealed class UserTypeDto
{
    public int UserTypeId { get; set; }
    public string UserType { get; set; } = string.Empty;
}

public sealed record CreateUserTypeRequest(int UserTypeId, string UserType);
public sealed record UpdateUserTypeRequest(string UserType);

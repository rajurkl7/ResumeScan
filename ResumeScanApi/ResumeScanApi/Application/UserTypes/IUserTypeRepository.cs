using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.UserTypes;

public interface IUserTypeRepository
{
    Task<IReadOnlyList<UserType>> GetAllAsync(CancellationToken cancellationToken);
    Task<UserType?> GetByIdAsync(int userTypeId, CancellationToken cancellationToken);
    Task<UserType?> CreateAsync(UserType userType, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(UserType userType, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int userTypeId, CancellationToken cancellationToken);
}

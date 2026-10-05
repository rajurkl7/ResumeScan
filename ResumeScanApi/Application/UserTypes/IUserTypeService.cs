using ResumeScanApi.Application.Contracts;

namespace ResumeScanApi.Application.UserTypes;

public interface IUserTypeService
{
    Task<ApiResponse<IReadOnlyList<UserTypeDto>>> GetAllAsync(CancellationToken cancellationToken);
    Task<ApiResponse<UserTypeDto>> GetByIdAsync(int userTypeId, CancellationToken cancellationToken);
    Task<ApiResponse<UserTypeDto>> CreateAsync(CreateUserTypeRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<UserTypeDto>> UpdateAsync(int userTypeId, UpdateUserTypeRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<bool>> DeleteAsync(int userTypeId, CancellationToken cancellationToken);
}

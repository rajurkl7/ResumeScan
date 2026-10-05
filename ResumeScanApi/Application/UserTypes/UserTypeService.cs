using AutoMapper;
using ResumeScanApi.Application.Contracts;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.UserTypes;

public sealed class UserTypeService(IUserTypeRepository repository, IMapper mapper) : IUserTypeService
{
    public async Task<ApiResponse<IReadOnlyList<UserTypeDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var userTypes = await repository.GetAllAsync(cancellationToken);
        return ApiResponse<IReadOnlyList<UserTypeDto>>.Ok(mapper.Map<IReadOnlyList<UserTypeDto>>(userTypes));
    }

    public async Task<ApiResponse<UserTypeDto>> GetByIdAsync(int userTypeId, CancellationToken cancellationToken)
    {
        var userType = await repository.GetByIdAsync(userTypeId, cancellationToken);
        return userType is null
            ? ApiResponse<UserTypeDto>.Fail("User type was not found.")
            : ApiResponse<UserTypeDto>.Ok(mapper.Map<UserTypeDto>(userType));
    }

    public async Task<ApiResponse<UserTypeDto>> CreateAsync(CreateUserTypeRequest request, CancellationToken cancellationToken)
    {
        var userType = mapper.Map<UserType>(request);
        var created = await repository.CreateAsync(userType, cancellationToken);
        return created is null
            ? ApiResponse<UserTypeDto>.Fail("User type could not be created.")
            : ApiResponse<UserTypeDto>.Ok(mapper.Map<UserTypeDto>(created), "User type created successfully.");
    }

    public async Task<ApiResponse<UserTypeDto>> UpdateAsync(int userTypeId, UpdateUserTypeRequest request, CancellationToken cancellationToken)
    {
        var userType = mapper.Map<UserType>(request);
        userType.UserTypeId = userTypeId;
        return await repository.UpdateAsync(userType, cancellationToken)
            ? ApiResponse<UserTypeDto>.Ok(mapper.Map<UserTypeDto>(userType), "User type updated successfully.")
            : ApiResponse<UserTypeDto>.Fail("User type was not found.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int userTypeId, CancellationToken cancellationToken)
    {
        return await repository.DeleteAsync(userTypeId, cancellationToken)
            ? ApiResponse<bool>.Ok(true, "User type deleted successfully.")
            : ApiResponse<bool>.Fail("User type was not found.");
    }
}

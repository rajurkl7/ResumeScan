using AutoMapper;
using ResumeScanApi.Application.Contracts;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.CoverageStatuses;

public sealed class CoverageStatusService(ICoverageStatusRepository repository, IMapper mapper) : ICoverageStatusService
{
    public async Task<ApiResponse<IReadOnlyList<CoverageStatusDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var coverageStatuses = await repository.GetAllAsync(cancellationToken);
        return ApiResponse<IReadOnlyList<CoverageStatusDto>>.Ok(mapper.Map<IReadOnlyList<CoverageStatusDto>>(coverageStatuses));
    }

    public async Task<ApiResponse<CoverageStatusDto>> GetByIdAsync(int coverageStatusId, CancellationToken cancellationToken)
    {
        var coverageStatus = await repository.GetByIdAsync(coverageStatusId, cancellationToken);
        return coverageStatus is null
            ? ApiResponse<CoverageStatusDto>.Fail("Coverage status was not found.")
            : ApiResponse<CoverageStatusDto>.Ok(mapper.Map<CoverageStatusDto>(coverageStatus));
    }

    public async Task<ApiResponse<CoverageStatusDto>> CreateAsync(CreateCoverageStatusRequest request, CancellationToken cancellationToken)
    {
        var coverageStatus = mapper.Map<CoverageStatus>(request);
        var created = await repository.CreateAsync(coverageStatus, cancellationToken);
        return created is null
            ? ApiResponse<CoverageStatusDto>.Fail("Coverage status could not be created.")
            : ApiResponse<CoverageStatusDto>.Ok(mapper.Map<CoverageStatusDto>(created), "Coverage status created successfully.");
    }

    public async Task<ApiResponse<CoverageStatusDto>> UpdateAsync(int coverageStatusId, UpdateCoverageStatusRequest request, CancellationToken cancellationToken)
    {
        var coverageStatus = mapper.Map<CoverageStatus>(request);
        coverageStatus.CoverageStatusId = coverageStatusId;
        return await repository.UpdateAsync(coverageStatus, cancellationToken)
            ? ApiResponse<CoverageStatusDto>.Ok(mapper.Map<CoverageStatusDto>(coverageStatus), "Coverage status updated successfully.")
            : ApiResponse<CoverageStatusDto>.Fail("Coverage status was not found.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int coverageStatusId, CancellationToken cancellationToken)
    {
        return await repository.DeleteAsync(coverageStatusId, cancellationToken)
            ? ApiResponse<bool>.Ok(true, "Coverage status deleted successfully.")
            : ApiResponse<bool>.Fail("Coverage status was not found.");
    }
}

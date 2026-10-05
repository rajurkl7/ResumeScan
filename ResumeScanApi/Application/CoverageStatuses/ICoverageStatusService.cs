using ResumeScanApi.Application.Contracts;

namespace ResumeScanApi.Application.CoverageStatuses;

public interface ICoverageStatusService
{
    Task<ApiResponse<IReadOnlyList<CoverageStatusDto>>> GetAllAsync(CancellationToken cancellationToken);
    Task<ApiResponse<CoverageStatusDto>> GetByIdAsync(int coverageStatusId, CancellationToken cancellationToken);
    Task<ApiResponse<CoverageStatusDto>> CreateAsync(CreateCoverageStatusRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<CoverageStatusDto>> UpdateAsync(int coverageStatusId, UpdateCoverageStatusRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<bool>> DeleteAsync(int coverageStatusId, CancellationToken cancellationToken);
}

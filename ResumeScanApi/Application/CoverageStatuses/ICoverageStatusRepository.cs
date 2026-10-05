using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.CoverageStatuses;

public interface ICoverageStatusRepository
{
    Task<IReadOnlyList<CoverageStatus>> GetAllAsync(CancellationToken cancellationToken);
    Task<CoverageStatus?> GetByIdAsync(int coverageStatusId, CancellationToken cancellationToken);
    Task<CoverageStatus?> CreateAsync(CoverageStatus coverageStatus, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(CoverageStatus coverageStatus, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int coverageStatusId, CancellationToken cancellationToken);
}

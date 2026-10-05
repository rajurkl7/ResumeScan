using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.Manufactures;

public interface IManufactureRepository
{
    Task<IReadOnlyList<Manufacture>> GetAllAsync(CancellationToken cancellationToken);
    Task<Manufacture?> GetByIdAsync(int manufactureId, CancellationToken cancellationToken);
    Task<Manufacture?> CreateAsync(Manufacture manufacture, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Manufacture manufacture, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int manufactureId, CancellationToken cancellationToken);
}

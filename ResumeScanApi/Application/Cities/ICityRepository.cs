using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.Cities;

public interface ICityRepository
{
    Task<IReadOnlyList<City>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<City>> GetForStatesAsync(
        IReadOnlyCollection<int> stateIds,
        CancellationToken cancellationToken);
    Task<City?> GetByIdAsync(int cityId, CancellationToken cancellationToken);
    Task<City?> CreateAsync(City city, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(City city, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int cityId, CancellationToken cancellationToken);
}

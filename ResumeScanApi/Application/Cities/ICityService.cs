namespace ResumeScanApi.Application.Cities;

public interface ICityService
{
    Task<Contracts.ApiResponse<IReadOnlyList<CityDto>>> GetAllAsync(
        CancellationToken cancellationToken,
        IReadOnlyCollection<int>? stateIds = null);
    Task<Contracts.ApiResponse<CityDto>> GetByIdAsync(int cityId, CancellationToken cancellationToken);
    Task<Contracts.ApiResponse<CityDto>> CreateAsync(CreateCityRequest request, CancellationToken cancellationToken);
    Task<Contracts.ApiResponse<CityDto>> UpdateAsync(int cityId, UpdateCityRequest request, CancellationToken cancellationToken);
    Task<Contracts.ApiResponse<bool>> DeleteAsync(int cityId, CancellationToken cancellationToken);
}

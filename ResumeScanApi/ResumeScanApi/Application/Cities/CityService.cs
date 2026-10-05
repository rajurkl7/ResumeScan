using AutoMapper;
using ResumeScanApi.Application.Contracts;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.Cities;

public sealed class CityService(ICityRepository repository, IMapper mapper) : ICityService
{
    public async Task<ApiResponse<IReadOnlyList<CityDto>>> GetAllAsync(
        CancellationToken cancellationToken,
        IReadOnlyCollection<int>? stateIds = null)
    {
        var cities = stateIds is { Count: > 0 }
            ? await repository.GetForStatesAsync(stateIds, cancellationToken)
            : await repository.GetAllAsync(cancellationToken);
        return ApiResponse<IReadOnlyList<CityDto>>.Ok(mapper.Map<IReadOnlyList<CityDto>>(cities));
    }

    public async Task<ApiResponse<CityDto>> GetByIdAsync(int cityId, CancellationToken cancellationToken)
    {
        var city = await repository.GetByIdAsync(cityId, cancellationToken);
        return city is null
            ? ApiResponse<CityDto>.Fail("City was not found.")
            : ApiResponse<CityDto>.Ok(mapper.Map<CityDto>(city));
    }

    public async Task<ApiResponse<CityDto>> CreateAsync(CreateCityRequest request, CancellationToken cancellationToken)
    {
        var city = mapper.Map<City>(request);
        var created = await repository.CreateAsync(city, cancellationToken);
        return created is null
            ? ApiResponse<CityDto>.Fail("City could not be created.")
            : ApiResponse<CityDto>.Ok(mapper.Map<CityDto>(created), "City created successfully.");
    }

    public async Task<ApiResponse<CityDto>> UpdateAsync(int cityId, UpdateCityRequest request, CancellationToken cancellationToken)
    {
        var city = mapper.Map<City>(request);
        city.CityId = cityId;
        return await repository.UpdateAsync(city, cancellationToken)
            ? ApiResponse<CityDto>.Ok(mapper.Map<CityDto>(city), "City updated successfully.")
            : ApiResponse<CityDto>.Fail("City was not found.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int cityId, CancellationToken cancellationToken)
        => await repository.DeleteAsync(cityId, cancellationToken)
            ? ApiResponse<bool>.Ok(true, "City deleted successfully.")
            : ApiResponse<bool>.Fail("City was not found.");
}

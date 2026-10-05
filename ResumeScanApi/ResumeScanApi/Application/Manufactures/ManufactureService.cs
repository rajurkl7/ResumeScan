using AutoMapper;
using ResumeScanApi.Application.Contracts;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.Manufactures;

public sealed class ManufactureService(IManufactureRepository repository, IMapper mapper) : IManufactureService
{
    public async Task<ApiResponse<IReadOnlyList<ManufactureDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var manufactures = await repository.GetAllAsync(cancellationToken);
        return ApiResponse<IReadOnlyList<ManufactureDto>>.Ok(mapper.Map<IReadOnlyList<ManufactureDto>>(manufactures));
    }

    public async Task<ApiResponse<ManufactureDto>> GetByIdAsync(int manufactureId, CancellationToken cancellationToken)
    {
        var manufacture = await repository.GetByIdAsync(manufactureId, cancellationToken);
        return manufacture is null
            ? ApiResponse<ManufactureDto>.Fail("Manufacture was not found.")
            : ApiResponse<ManufactureDto>.Ok(mapper.Map<ManufactureDto>(manufacture));
    }

    public async Task<ApiResponse<ManufactureDto>> CreateAsync(CreateManufactureRequest request, CancellationToken cancellationToken)
    {
        var manufacture = mapper.Map<Manufacture>(request);
        var created = await repository.CreateAsync(manufacture, cancellationToken);
        return created is null
            ? ApiResponse<ManufactureDto>.Fail("Manufacture could not be created.")
            : ApiResponse<ManufactureDto>.Ok(mapper.Map<ManufactureDto>(created), "Manufacture created successfully.");
    }

    public async Task<ApiResponse<ManufactureDto>> UpdateAsync(int manufactureId, UpdateManufactureRequest request, CancellationToken cancellationToken)
    {
        var manufacture = mapper.Map<Manufacture>(request);
        manufacture.ManufactureId = manufactureId;
        return await repository.UpdateAsync(manufacture, cancellationToken)
            ? ApiResponse<ManufactureDto>.Ok(mapper.Map<ManufactureDto>(manufacture), "Manufacture updated successfully.")
            : ApiResponse<ManufactureDto>.Fail("Manufacture was not found.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int manufactureId, CancellationToken cancellationToken)
    {
        return await repository.DeleteAsync(manufactureId, cancellationToken)
            ? ApiResponse<bool>.Ok(true, "Manufacture deleted successfully.")
            : ApiResponse<bool>.Fail("Manufacture was not found.");
    }
}

using AutoMapper;
using ResumeScanApi.Application.Contracts;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.EquipmentModalities;

public sealed class EquipmentModalityService(IEquipmentModalityRepository repository, IMapper mapper) : IEquipmentModalityService
{
    public async Task<ApiResponse<IReadOnlyList<EquipmentModalityDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var equipmentModalities = await repository.GetAllAsync(cancellationToken);
        return ApiResponse<IReadOnlyList<EquipmentModalityDto>>.Ok(mapper.Map<IReadOnlyList<EquipmentModalityDto>>(equipmentModalities));
    }

    public async Task<ApiResponse<EquipmentModalityDto>> GetByIdAsync(int equipmentModalityId, CancellationToken cancellationToken)
    {
        var equipmentModality = await repository.GetByIdAsync(equipmentModalityId, cancellationToken);
        return equipmentModality is null
            ? ApiResponse<EquipmentModalityDto>.Fail("Equipment modality was not found.")
            : ApiResponse<EquipmentModalityDto>.Ok(mapper.Map<EquipmentModalityDto>(equipmentModality));
    }

    public async Task<ApiResponse<EquipmentModalityDto>> CreateAsync(CreateEquipmentModalityRequest request, CancellationToken cancellationToken)
    {
        var equipmentModality = mapper.Map<EquipmentModality>(request);
        var created = await repository.CreateAsync(equipmentModality, cancellationToken);
        return created is null
            ? ApiResponse<EquipmentModalityDto>.Fail("Equipment modality could not be created.")
            : ApiResponse<EquipmentModalityDto>.Ok(mapper.Map<EquipmentModalityDto>(created), "Equipment modality created successfully.");
    }

    public async Task<ApiResponse<EquipmentModalityDto>> UpdateAsync(int equipmentModalityId, UpdateEquipmentModalityRequest request, CancellationToken cancellationToken)
    {
        var equipmentModality = mapper.Map<EquipmentModality>(request);
        equipmentModality.EquipmentModalityId = equipmentModalityId;
        return await repository.UpdateAsync(equipmentModality, cancellationToken)
            ? ApiResponse<EquipmentModalityDto>.Ok(mapper.Map<EquipmentModalityDto>(equipmentModality), "Equipment modality updated successfully.")
            : ApiResponse<EquipmentModalityDto>.Fail("Equipment modality was not found.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int equipmentModalityId, CancellationToken cancellationToken)
    {
        return await repository.DeleteAsync(equipmentModalityId, cancellationToken)
            ? ApiResponse<bool>.Ok(true, "Equipment modality deleted successfully.")
            : ApiResponse<bool>.Fail("Equipment modality was not found.");
    }
}

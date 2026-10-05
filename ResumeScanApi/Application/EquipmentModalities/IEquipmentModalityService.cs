using ResumeScanApi.Application.Contracts;

namespace ResumeScanApi.Application.EquipmentModalities;

public interface IEquipmentModalityService
{
    Task<ApiResponse<IReadOnlyList<EquipmentModalityDto>>> GetAllAsync(CancellationToken cancellationToken);
    Task<ApiResponse<EquipmentModalityDto>> GetByIdAsync(int equipmentModalityId, CancellationToken cancellationToken);
    Task<ApiResponse<EquipmentModalityDto>> CreateAsync(CreateEquipmentModalityRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<EquipmentModalityDto>> UpdateAsync(int equipmentModalityId, UpdateEquipmentModalityRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<bool>> DeleteAsync(int equipmentModalityId, CancellationToken cancellationToken);
}

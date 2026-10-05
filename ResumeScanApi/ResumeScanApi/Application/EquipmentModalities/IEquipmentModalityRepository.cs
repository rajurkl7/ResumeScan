using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.EquipmentModalities;

public interface IEquipmentModalityRepository
{
    Task<IReadOnlyList<EquipmentModality>> GetAllAsync(CancellationToken cancellationToken);
    Task<EquipmentModality?> GetByIdAsync(int equipmentModalityId, CancellationToken cancellationToken);
    Task<EquipmentModality?> CreateAsync(EquipmentModality equipmentModality, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(EquipmentModality equipmentModality, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int equipmentModalityId, CancellationToken cancellationToken);
}

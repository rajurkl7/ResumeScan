using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.FieldServiceEngineers;

public interface IFieldServiceEngineerRepository
{
    Task<IReadOnlyList<FieldServiceEngineer>> GetAllAsync(CancellationToken cancellationToken);
    Task<FieldServiceEngineer?> GetByIdAsync(int fieldServiceEngineerId, CancellationToken cancellationToken);
    Task<FieldServiceEngineer?> CreateAsync(FieldServiceEngineer engineer, CancellationToken cancellationToken);
    Task<FieldServiceEngineer?> FindByEmailAsync(string email, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(FieldServiceEngineer engineer, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int fieldServiceEngineerId, CancellationToken cancellationToken);
}

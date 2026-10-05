using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.States;

public interface IStateRepository
{
    Task<IReadOnlyList<State>> GetAllAsync(CancellationToken cancellationToken);
    Task<State?> GetByIdAsync(int stateId, CancellationToken cancellationToken);
    Task<State?> CreateAsync(State state, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(State state, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int stateId, CancellationToken cancellationToken);
}

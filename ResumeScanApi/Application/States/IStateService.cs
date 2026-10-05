namespace ResumeScanApi.Application.States;

public interface IStateService
{
    Task<Contracts.ApiResponse<IReadOnlyList<StateDto>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Contracts.ApiResponse<StateDto>> GetByIdAsync(int stateId, CancellationToken cancellationToken);
    Task<Contracts.ApiResponse<StateDto>> CreateAsync(CreateStateRequest request, CancellationToken cancellationToken);
    Task<Contracts.ApiResponse<StateDto>> UpdateAsync(int stateId, UpdateStateRequest request, CancellationToken cancellationToken);
    Task<Contracts.ApiResponse<bool>> DeleteAsync(int stateId, CancellationToken cancellationToken);
}

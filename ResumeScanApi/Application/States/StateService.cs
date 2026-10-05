using AutoMapper;
using ResumeScanApi.Application.Contracts;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.States;

public sealed class StateService(IStateRepository repository, IMapper mapper) : IStateService
{
    public async Task<ApiResponse<IReadOnlyList<StateDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var states = await repository.GetAllAsync(cancellationToken);
        return ApiResponse<IReadOnlyList<StateDto>>.Ok(mapper.Map<IReadOnlyList<StateDto>>(states));
    }

    public async Task<ApiResponse<StateDto>> GetByIdAsync(int stateId, CancellationToken cancellationToken)
    {
        var state = await repository.GetByIdAsync(stateId, cancellationToken);
        return state is null
            ? ApiResponse<StateDto>.Fail("State was not found.")
            : ApiResponse<StateDto>.Ok(mapper.Map<StateDto>(state));
    }

    public async Task<ApiResponse<StateDto>> CreateAsync(CreateStateRequest request, CancellationToken cancellationToken)
    {
        var state = mapper.Map<State>(request);
        var created = await repository.CreateAsync(state, cancellationToken);
        return created is null
            ? ApiResponse<StateDto>.Fail("State could not be created.")
            : ApiResponse<StateDto>.Ok(mapper.Map<StateDto>(created), "State created successfully.");
    }

    public async Task<ApiResponse<StateDto>> UpdateAsync(int stateId, UpdateStateRequest request, CancellationToken cancellationToken)
    {
        var state = mapper.Map<State>(request);
        state.StateId = stateId;
        return await repository.UpdateAsync(state, cancellationToken)
            ? ApiResponse<StateDto>.Ok(mapper.Map<StateDto>(state), "State updated successfully.")
            : ApiResponse<StateDto>.Fail("State was not found.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int stateId, CancellationToken cancellationToken)
        => await repository.DeleteAsync(stateId, cancellationToken)
            ? ApiResponse<bool>.Ok(true, "State deleted successfully.")
            : ApiResponse<bool>.Fail("State was not found.");
}

using System.Data;
using Dapper;
using ResumeScanApi.Application.States;
using ResumeScanApi.Domain.Entities;
using ResumeScanApi.Infrastructure.Data;

namespace ResumeScanApi.Infrastructure.Repositories;

public sealed class StateRepository(ISqlConnectionFactory connectionFactory) : IStateRepository
{
    public async Task<IReadOnlyList<State>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.State_GetAll", commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<State>(command);
        return rows.AsList();
    }

    public async Task<State?> GetByIdAsync(int stateId, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.State_GetById", new { StateId = stateId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<State>(command);
    }

    public async Task<State?> CreateAsync(State state, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.State_Create", new { state.StateId, state.StateName, state.IsEnabled, state.CreatedDateTime }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<State>(command);
    }

    public async Task<bool> UpdateAsync(State state, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.State_Update", new { state.StateId, state.StateName, state.IsEnabled }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }

    public async Task<bool> DeleteAsync(int stateId, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.State_Delete", new { StateId = stateId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }
}

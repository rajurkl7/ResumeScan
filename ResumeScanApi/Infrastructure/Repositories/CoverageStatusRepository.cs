using System.Data;
using Dapper;
using ResumeScanApi.Application.CoverageStatuses;
using ResumeScanApi.Domain.Entities;
using ResumeScanApi.Infrastructure.Data;

namespace ResumeScanApi.Infrastructure.Repositories;

public sealed class CoverageStatusRepository(ISqlConnectionFactory connectionFactory) : ICoverageStatusRepository
{
    public async Task<IReadOnlyList<CoverageStatus>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.CoverageStatus_GetAll", commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<CoverageStatus>(command);
        return rows.AsList();
    }

    public async Task<CoverageStatus?> GetByIdAsync(int coverageStatusId, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.CoverageStatus_GetById", new { CoverageStatusId = coverageStatusId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<CoverageStatus>(command);
    }

    public async Task<CoverageStatus?> CreateAsync(CoverageStatus coverageStatus, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            "Master.CoverageStatus_Create",
            new { coverageStatus.CoverageStatusId, coverageStatus.CoverageStatusName, coverageStatus.IsEnabled, coverageStatus.CreatedDateTime },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<CoverageStatus>(command);
    }

    public async Task<bool> UpdateAsync(CoverageStatus coverageStatus, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            "Master.CoverageStatus_Update",
            new { coverageStatus.CoverageStatusId, coverageStatus.CoverageStatusName, coverageStatus.IsEnabled },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }

    public async Task<bool> DeleteAsync(int coverageStatusId, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.CoverageStatus_Delete", new { CoverageStatusId = coverageStatusId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }
}

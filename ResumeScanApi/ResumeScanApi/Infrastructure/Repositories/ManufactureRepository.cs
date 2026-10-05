using System.Data;
using Dapper;
using ResumeScanApi.Application.Manufactures;
using ResumeScanApi.Domain.Entities;
using ResumeScanApi.Infrastructure.Data;

namespace ResumeScanApi.Infrastructure.Repositories;

public sealed class ManufactureRepository(ISqlConnectionFactory connectionFactory) : IManufactureRepository
{
    public async Task<IReadOnlyList<Manufacture>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.Manufacture_GetAll", commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<Manufacture>(command);
        return rows.AsList();
    }

    public async Task<Manufacture?> GetByIdAsync(int manufactureId, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.Manufacture_GetById", new { ManufactureId = manufactureId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<Manufacture>(command);
    }

    public async Task<Manufacture?> CreateAsync(Manufacture manufacture, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            "Master.Manufacture_Create",
            new { manufacture.ManufactureId, manufacture.ManufactureName, manufacture.IsEnabled, manufacture.CreatedDateTime },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<Manufacture>(command);
    }

    public async Task<bool> UpdateAsync(Manufacture manufacture, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            "Master.Manufacture_Update",
            new { manufacture.ManufactureId, manufacture.ManufactureName, manufacture.IsEnabled },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }

    public async Task<bool> DeleteAsync(int manufactureId, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.Manufacture_Delete", new { ManufactureId = manufactureId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }
}

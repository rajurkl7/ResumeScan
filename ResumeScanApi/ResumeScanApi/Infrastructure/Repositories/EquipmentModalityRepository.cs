using System.Data;
using Dapper;
using ResumeScanApi.Application.EquipmentModalities;
using ResumeScanApi.Domain.Entities;
using ResumeScanApi.Infrastructure.Data;

namespace ResumeScanApi.Infrastructure.Repositories;

public sealed class EquipmentModalityRepository(ISqlConnectionFactory connectionFactory) : IEquipmentModalityRepository
{
    public async Task<IReadOnlyList<EquipmentModality>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.EquipmentModality_GetAll", commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<EquipmentModality>(command);
        return rows.AsList();
    }

    public async Task<EquipmentModality?> GetByIdAsync(int equipmentModalityId, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.EquipmentModality_GetById", new { EquipmentModalityId = equipmentModalityId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<EquipmentModality>(command);
    }

    public async Task<EquipmentModality?> CreateAsync(EquipmentModality equipmentModality, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            "Master.EquipmentModality_Create",
            new { equipmentModality.EquipmentModalityId, equipmentModality.EquipmentModalityName, equipmentModality.IsEnabled, equipmentModality.CreatedDateTime },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<EquipmentModality>(command);
    }

    public async Task<bool> UpdateAsync(EquipmentModality equipmentModality, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            "Master.EquipmentModality_Update",
            new { equipmentModality.EquipmentModalityId, equipmentModality.EquipmentModalityName, equipmentModality.IsEnabled },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }

    public async Task<bool> DeleteAsync(int equipmentModalityId, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.EquipmentModality_Delete", new { EquipmentModalityId = equipmentModalityId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }
}

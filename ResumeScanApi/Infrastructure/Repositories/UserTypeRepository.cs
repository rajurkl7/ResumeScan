using System.Data;
using Dapper;
using ResumeScanApi.Application.UserTypes;
using ResumeScanApi.Domain.Entities;
using ResumeScanApi.Infrastructure.Data;

namespace ResumeScanApi.Infrastructure.Repositories;

public sealed class UserTypeRepository(ISqlConnectionFactory connectionFactory) : IUserTypeRepository
{
    public async Task<IReadOnlyList<UserType>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.UserType_GetAll", commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<UserType>(command);
        return rows.AsList();
    }

    public async Task<UserType?> GetByIdAsync(int userTypeId, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.UserType_GetById", new { UserTypeId = userTypeId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<UserType>(command);
    }

    public async Task<UserType?> CreateAsync(UserType userType, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.UserType_Create", new { userType.UserTypeId, UserType = userType.UserTypeName }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<UserType>(command);
    }

    public async Task<bool> UpdateAsync(UserType userType, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.UserType_Update", new { userType.UserTypeId, UserType = userType.UserTypeName }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }

    public async Task<bool> DeleteAsync(int userTypeId, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.UserType_Delete", new { UserTypeId = userTypeId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }
}

using System.Data;
using Dapper;
using ResumeScanApi.Application.Cities;
using ResumeScanApi.Domain.Entities;
using ResumeScanApi.Infrastructure.Data;

namespace ResumeScanApi.Infrastructure.Repositories;

public sealed class CityRepository(ISqlConnectionFactory connectionFactory) : ICityRepository
{
    public async Task<IReadOnlyList<City>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.City_GetAll", commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<City>(command);
        return rows.AsList();
    }

    public async Task<IReadOnlyList<City>> GetForStatesAsync(
        IReadOnlyCollection<int> stateIds,
        CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            """
            SELECT c.CityId, c.StateId, s.StateName, c.CityName, c.IsEnabled, c.CreatedDateTime
            FROM Master.City c
            INNER JOIN Master.State s ON s.StateId = c.StateId
            WHERE c.StateId IN @StateIds
            ORDER BY c.CityName, s.StateName;
            """,
            new { StateIds = stateIds },
            cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<City>(command);
        return rows.AsList();
    }

    public async Task<City?> GetByIdAsync(int cityId, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.City_GetById", new { CityId = cityId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<City>(command);
    }

    public async Task<City?> CreateAsync(City city, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.City_Create", new { city.CityId, city.StateId, city.CityName, city.IsEnabled, city.CreatedDateTime }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<City>(command);
    }

    public async Task<bool> UpdateAsync(City city, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.City_Update", new { city.CityId, city.StateId, city.CityName, city.IsEnabled }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }

    public async Task<bool> DeleteAsync(int cityId, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.City_Delete", new { CityId = cityId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }
}

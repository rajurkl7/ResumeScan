using System.Data;
using System.Text.Json;
using Dapper;
using ResumeScanApi.Application.FieldServiceEngineers;
using ResumeScanApi.Domain.Entities;
using ResumeScanApi.Infrastructure.Data;

namespace ResumeScanApi.Infrastructure.Repositories;

public sealed class FieldServiceEngineerRepository(ISqlConnectionFactory connectionFactory) : IFieldServiceEngineerRepository
{
    public async Task<IReadOnlyList<FieldServiceEngineer>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Engineer.FieldServiceEngineer_GetAll", commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<FieldServiceEngineer>(command);
        return rows.Select(Hydrate).ToList();
    }

    public async Task<FieldServiceEngineer?> GetByIdAsync(int fieldServiceEngineerId, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Engineer.FieldServiceEngineer_GetById", new { FieldServiceEngineerId = fieldServiceEngineerId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var engineer = await connection.QuerySingleOrDefaultAsync<FieldServiceEngineer>(command);
        return engineer is null ? null : Hydrate(engineer);
    }

    public async Task<FieldServiceEngineer?> CreateAsync(FieldServiceEngineer engineer, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            "Engineer.FieldServiceEngineer_Create",
            new
            {
                engineer.EngineerName,
                engineer.BaseLocation,
                engineer.MobileNumber,
                engineer.EmailAddress,
                engineer.PasswordHash,
                engineer.EmploymentStatus,
                engineer.IsEnabled,
                engineer.CreatedDateTime,
                CapabilitiesJson = JsonSerializer.Serialize(engineer.Capabilities),
                StatesJson = JsonSerializer.Serialize(engineer.OperatingStates),
                CitiesJson = JsonSerializer.Serialize(engineer.OperatingCities),
                DocumentsJson = JsonSerializer.Serialize(engineer.Documents)
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        var created = await connection.QuerySingleOrDefaultAsync<FieldServiceEngineer>(command);
        return created is null ? null : Hydrate(created);
    }

    public async Task<FieldServiceEngineer?> FindByEmailAsync(string email, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            "SELECT TOP 1 e.*, (SELECT EquipmentModalityId, ManufactureId FROM Engineer.FieldServiceEngineerCapability WHERE FieldServiceEngineerId = e.FieldServiceEngineerId FOR JSON PATH) AS CapabilitiesJson, (SELECT es.StateId, s.StateName FROM Engineer.FieldServiceEngineerState es INNER JOIN Master.State s ON s.StateId = es.StateId WHERE es.FieldServiceEngineerId = e.FieldServiceEngineerId FOR JSON PATH) AS StatesJson, (SELECT ec.CityId, c.CityName, c.StateId, s.StateName FROM Engineer.FieldServiceEngineerCity ec INNER JOIN Master.City c ON c.CityId = ec.CityId INNER JOIN Master.State s ON s.StateId = c.StateId WHERE ec.FieldServiceEngineerId = e.FieldServiceEngineerId FOR JSON PATH) AS CitiesJson, (SELECT FieldServiceEngineerDocumentId, DocumentType, OriginalFileName, StoredFilePath, ContentType, FileSizeBytes, UploadedDateTime FROM Engineer.FieldServiceEngineerDocument WHERE FieldServiceEngineerId = e.FieldServiceEngineerId FOR JSON PATH) AS DocumentsJson FROM Engineer.FieldServiceEngineer e WHERE EmailAddress = @Email AND IsEnabled = 1",
            new { Email = email },
            cancellationToken: cancellationToken);
        var engineer = await connection.QuerySingleOrDefaultAsync<FieldServiceEngineer>(command);
        return engineer is null ? null : Hydrate(engineer);
    }

    public async Task<bool> UpdateAsync(FieldServiceEngineer engineer, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            "Engineer.FieldServiceEngineer_Update",
            new
            {
                engineer.FieldServiceEngineerId,
                engineer.EngineerName,
                engineer.BaseLocation,
                engineer.MobileNumber,
                engineer.EmailAddress,
                engineer.PasswordHash,
                engineer.EmploymentStatus,
                engineer.IsEnabled,
                CapabilitiesJson = JsonSerializer.Serialize(engineer.Capabilities),
                StatesJson = JsonSerializer.Serialize(engineer.OperatingStates),
                CitiesJson = JsonSerializer.Serialize(engineer.OperatingCities),
                DocumentsJson = JsonSerializer.Serialize(engineer.Documents)
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }

    private static FieldServiceEngineer Hydrate(FieldServiceEngineer engineer)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        engineer.Capabilities = Deserialize<List<FieldServiceEngineerCapability>>(engineer.CapabilitiesJson, options);
        engineer.OperatingStates = Deserialize<List<FieldServiceEngineerState>>(engineer.StatesJson, options);
        engineer.OperatingCities = Deserialize<List<FieldServiceEngineerCity>>(engineer.CitiesJson, options);
        engineer.Documents = Deserialize<List<FieldServiceEngineerDocument>>(engineer.DocumentsJson, options);
        return engineer;
    }

    private static T Deserialize<T>(string? json, JsonSerializerOptions options) where T : new()
        => string.IsNullOrWhiteSpace(json) ? new T() : JsonSerializer.Deserialize<T>(json, options) ?? new T();

    public async Task<bool> DeleteAsync(int fieldServiceEngineerId, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Engineer.FieldServiceEngineer_Delete", new { FieldServiceEngineerId = fieldServiceEngineerId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }
}

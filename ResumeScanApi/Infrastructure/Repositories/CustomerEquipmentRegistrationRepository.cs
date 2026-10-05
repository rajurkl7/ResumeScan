using System.Data;
using Dapper;
using ResumeScanApi.Application.CustomerEquipmentRegistrations;
using ResumeScanApi.Domain.Entities;
using ResumeScanApi.Infrastructure.Data;

namespace ResumeScanApi.Infrastructure.Repositories;

public sealed class CustomerEquipmentRegistrationRepository(ISqlConnectionFactory connectionFactory) : ICustomerEquipmentRegistrationRepository
{
    public async Task<IReadOnlyList<CustomerEquipmentRegistration>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Customer.CustomerEquipmentRegistration_GetAll", commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<CustomerEquipmentRegistration>(command);
        return rows.AsList();
    }

    public async Task<CustomerEquipmentRegistration?> GetByIdAsync(int customerEquipmentRegistrationId, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Customer.CustomerEquipmentRegistration_GetById", new { CustomerEquipmentRegistrationId = customerEquipmentRegistrationId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<CustomerEquipmentRegistration>(command);
    }

    public async Task<CustomerEquipmentRegistration?> CreateAsync(CustomerEquipmentRegistration registration, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            "Customer.CustomerEquipmentRegistration_Create",
            new
            {
                registration.FacilityName,
                registration.CustomerContactName,
                registration.CustomerContactMobile,
                registration.PrimaryContactEmail,
                registration.Password,
                registration.FacilityAddress,
                registration.EquipmentModalityId,
                registration.ManufactureId,
                registration.ModelIdentifier,
                registration.EquipmentSerialNumber,
                registration.SoftwareFirmwareVersion,
                registration.CoverageStatusId,
                registration.IsEnabled,
                registration.CreatedDateTime
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<CustomerEquipmentRegistration>(command);
    }

    public async Task<CustomerEquipmentRegistration?> FindByCredentialsAsync(string email, string password, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            "SELECT TOP 1 * FROM Customer.CustomerEquipmentRegistration WHERE PrimaryContactEmail = @Email AND Password = @Password AND IsEnabled = 1",
            new { Email = email, Password = password },
            cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<CustomerEquipmentRegistration>(command);
    }

    public async Task<bool> UpdateAsync(CustomerEquipmentRegistration registration, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            "Customer.CustomerEquipmentRegistration_Update",
            new
            {
                registration.CustomerEquipmentRegistrationId,
                registration.FacilityName,
                registration.CustomerContactName,
                registration.CustomerContactMobile,
                registration.PrimaryContactEmail,
                registration.Password,
                registration.FacilityAddress,
                registration.EquipmentModalityId,
                registration.ManufactureId,
                registration.ModelIdentifier,
                registration.EquipmentSerialNumber,
                registration.SoftwareFirmwareVersion,
                registration.CoverageStatusId,
                registration.IsEnabled
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }

    public async Task<bool> DeleteAsync(int customerEquipmentRegistrationId, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Customer.CustomerEquipmentRegistration_Delete", new { CustomerEquipmentRegistrationId = customerEquipmentRegistrationId }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }
}

using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.CustomerEquipmentRegistrations;

public interface ICustomerEquipmentRegistrationRepository
{
    Task<IReadOnlyList<CustomerEquipmentRegistration>> GetAllAsync(CancellationToken cancellationToken);
    Task<CustomerEquipmentRegistration?> GetByIdAsync(int customerEquipmentRegistrationId, CancellationToken cancellationToken);
    Task<CustomerEquipmentRegistration?> CreateAsync(CustomerEquipmentRegistration registration, CancellationToken cancellationToken);
    Task<CustomerEquipmentRegistration?> FindByCredentialsAsync(string email, string password, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(CustomerEquipmentRegistration registration, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int customerEquipmentRegistrationId, CancellationToken cancellationToken);
}

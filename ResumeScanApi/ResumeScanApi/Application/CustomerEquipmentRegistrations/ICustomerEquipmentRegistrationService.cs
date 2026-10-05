using ResumeScanApi.Application.Contracts;

namespace ResumeScanApi.Application.CustomerEquipmentRegistrations;

public interface ICustomerEquipmentRegistrationService
{
    Task<ApiResponse<IReadOnlyList<CustomerEquipmentRegistrationDto>>> GetAllAsync(CancellationToken cancellationToken);
    Task<ApiResponse<CustomerEquipmentRegistrationDto>> GetByIdAsync(int customerEquipmentRegistrationId, CancellationToken cancellationToken);
    Task<ApiResponse<CustomerEquipmentRegistrationDto>> CreateAsync(CreateCustomerEquipmentRegistrationRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<CustomerEquipmentRegistrationDto>> AuthenticateAsync(CustomerLoginRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<CustomerEquipmentRegistrationDto>> UpdateAsync(int customerEquipmentRegistrationId, UpdateCustomerEquipmentRegistrationRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<bool>> DeleteAsync(int customerEquipmentRegistrationId, CancellationToken cancellationToken);
}

using AutoMapper;
using ResumeScanApi.Application.Contracts;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.CustomerEquipmentRegistrations;

public sealed class CustomerEquipmentRegistrationService(ICustomerEquipmentRegistrationRepository repository, IMapper mapper) : ICustomerEquipmentRegistrationService
{
    public async Task<ApiResponse<IReadOnlyList<CustomerEquipmentRegistrationDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var registrations = await repository.GetAllAsync(cancellationToken);
        return ApiResponse<IReadOnlyList<CustomerEquipmentRegistrationDto>>.Ok(mapper.Map<IReadOnlyList<CustomerEquipmentRegistrationDto>>(registrations));
    }

    public async Task<ApiResponse<CustomerEquipmentRegistrationDto>> GetByIdAsync(int customerEquipmentRegistrationId, CancellationToken cancellationToken)
    {
        var registration = await repository.GetByIdAsync(customerEquipmentRegistrationId, cancellationToken);
        return registration is null
            ? ApiResponse<CustomerEquipmentRegistrationDto>.Fail("Customer equipment registration was not found.")
            : ApiResponse<CustomerEquipmentRegistrationDto>.Ok(mapper.Map<CustomerEquipmentRegistrationDto>(registration));
    }

    public async Task<ApiResponse<CustomerEquipmentRegistrationDto>> CreateAsync(CreateCustomerEquipmentRegistrationRequest request, CancellationToken cancellationToken)
    {
        var registration = mapper.Map<CustomerEquipmentRegistration>(request);
        var created = await repository.CreateAsync(registration, cancellationToken);
        return created is null
            ? ApiResponse<CustomerEquipmentRegistrationDto>.Fail("Customer equipment registration could not be created.")
            : ApiResponse<CustomerEquipmentRegistrationDto>.Ok(mapper.Map<CustomerEquipmentRegistrationDto>(created), "Customer equipment registration created successfully.");
    }

    public async Task<ApiResponse<CustomerEquipmentRegistrationDto>> AuthenticateAsync(CustomerLoginRequest request, CancellationToken cancellationToken)
    {
        var registration = await repository.FindByCredentialsAsync(request.Email, request.Password, cancellationToken);
        return registration is null
            ? ApiResponse<CustomerEquipmentRegistrationDto>.Fail("Invalid email or password.")
            : ApiResponse<CustomerEquipmentRegistrationDto>.Ok(mapper.Map<CustomerEquipmentRegistrationDto>(registration));
    }

    public async Task<ApiResponse<CustomerEquipmentRegistrationDto>> UpdateAsync(int customerEquipmentRegistrationId, UpdateCustomerEquipmentRegistrationRequest request, CancellationToken cancellationToken)
    {
        var registration = mapper.Map<CustomerEquipmentRegistration>(request);
        registration.CustomerEquipmentRegistrationId = customerEquipmentRegistrationId;
        return await repository.UpdateAsync(registration, cancellationToken)
            ? ApiResponse<CustomerEquipmentRegistrationDto>.Ok(mapper.Map<CustomerEquipmentRegistrationDto>(registration), "Customer equipment registration updated successfully.")
            : ApiResponse<CustomerEquipmentRegistrationDto>.Fail("Customer equipment registration was not found.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int customerEquipmentRegistrationId, CancellationToken cancellationToken)
    {
        return await repository.DeleteAsync(customerEquipmentRegistrationId, cancellationToken)
            ? ApiResponse<bool>.Ok(true, "Customer equipment registration deleted successfully.")
            : ApiResponse<bool>.Fail("Customer equipment registration was not found.");
    }
}

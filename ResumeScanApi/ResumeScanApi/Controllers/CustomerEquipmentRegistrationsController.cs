using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeScanApi.Application.CustomerEquipmentRegistrations;

namespace ResumeScanApi.Controllers;

[ApiController]
[Route("api/customer-equipment-registrations")]
[Authorize]
public sealed class CustomerEquipmentRegistrationsController(ICustomerEquipmentRegistrationService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await service.GetAllAsync(cancellationToken));

    [HttpGet("{customerEquipmentRegistrationId:int}")]
    public async Task<IActionResult> GetById(int customerEquipmentRegistrationId, CancellationToken cancellationToken)
    {
        var response = await service.GetByIdAsync(customerEquipmentRegistrationId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerEquipmentRegistrationRequest request, CancellationToken cancellationToken)
    {
        var response = await service.CreateAsync(request, cancellationToken);
        return response.Success
            ? CreatedAtAction(nameof(GetById), new { customerEquipmentRegistrationId = response.Data!.CustomerEquipmentRegistrationId }, response)
            : Conflict(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(CustomerLoginRequest request, CancellationToken cancellationToken)
    {
        var response = await service.AuthenticateAsync(request, cancellationToken);
        return response.Success ? Ok(response) : Unauthorized(response);
    }

    [HttpPut("{customerEquipmentRegistrationId:int}")]
    public async Task<IActionResult> Update(int customerEquipmentRegistrationId, UpdateCustomerEquipmentRegistrationRequest request, CancellationToken cancellationToken)
    {
        var response = await service.UpdateAsync(customerEquipmentRegistrationId, request, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpDelete("{customerEquipmentRegistrationId:int}")]
    public async Task<IActionResult> Delete(int customerEquipmentRegistrationId, CancellationToken cancellationToken)
    {
        var response = await service.DeleteAsync(customerEquipmentRegistrationId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }
}

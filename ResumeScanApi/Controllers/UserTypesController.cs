using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeScanApi.Application.UserTypes;

namespace ResumeScanApi.Controllers;

[ApiController]
[Route("api/user-types")]
[Authorize]
public sealed class UserTypesController(IUserTypeService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await service.GetAllAsync(cancellationToken));

    [HttpGet("{userTypeId:int}")]
    public async Task<IActionResult> GetById(int userTypeId, CancellationToken cancellationToken)
    {
        var response = await service.GetByIdAsync(userTypeId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserTypeRequest request, CancellationToken cancellationToken)
    {
        var response = await service.CreateAsync(request, cancellationToken);
        return response.Success
            ? CreatedAtAction(nameof(GetById), new { userTypeId = response.Data!.UserTypeId }, response)
            : Conflict(response);
    }

    [HttpPut("{userTypeId:int}")]
    public async Task<IActionResult> Update(int userTypeId, UpdateUserTypeRequest request, CancellationToken cancellationToken)
    {
        var response = await service.UpdateAsync(userTypeId, request, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpDelete("{userTypeId:int}")]
    public async Task<IActionResult> Delete(int userTypeId, CancellationToken cancellationToken)
    {
        var response = await service.DeleteAsync(userTypeId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }
}

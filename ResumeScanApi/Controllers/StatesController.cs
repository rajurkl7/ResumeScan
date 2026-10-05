using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeScanApi.Application.States;

namespace ResumeScanApi.Controllers;

[ApiController]
[Route("api/states")]
[Authorize]
public sealed class StatesController(IStateService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await service.GetAllAsync(cancellationToken));

    [HttpGet("{stateId:int}")]
    public async Task<IActionResult> GetById(int stateId, CancellationToken cancellationToken)
    {
        var response = await service.GetByIdAsync(stateId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateStateRequest request, CancellationToken cancellationToken)
    {
        var response = await service.CreateAsync(request, cancellationToken);
        return response.Success
            ? CreatedAtAction(nameof(GetById), new { stateId = response.Data!.StateId }, response)
            : Conflict(response);
    }

    [HttpPut("{stateId:int}")]
    public async Task<IActionResult> Update(int stateId, UpdateStateRequest request, CancellationToken cancellationToken)
    {
        var response = await service.UpdateAsync(stateId, request, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpDelete("{stateId:int}")]
    public async Task<IActionResult> Delete(int stateId, CancellationToken cancellationToken)
    {
        var response = await service.DeleteAsync(stateId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }
}

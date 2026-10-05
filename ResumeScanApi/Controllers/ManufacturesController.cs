using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeScanApi.Application.Manufactures;

namespace ResumeScanApi.Controllers;

[ApiController]
[Route("api/manufactures")]
[Authorize]
public sealed class ManufacturesController(IManufactureService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await service.GetAllAsync(cancellationToken));

    [HttpGet("{manufactureId:int}")]
    public async Task<IActionResult> GetById(int manufactureId, CancellationToken cancellationToken)
    {
        var response = await service.GetByIdAsync(manufactureId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateManufactureRequest request, CancellationToken cancellationToken)
    {
        var response = await service.CreateAsync(request, cancellationToken);
        return response.Success
            ? CreatedAtAction(nameof(GetById), new { manufactureId = response.Data!.ManufactureId }, response)
            : Conflict(response);
    }

    [HttpPut("{manufactureId:int}")]
    public async Task<IActionResult> Update(int manufactureId, UpdateManufactureRequest request, CancellationToken cancellationToken)
    {
        var response = await service.UpdateAsync(manufactureId, request, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpDelete("{manufactureId:int}")]
    public async Task<IActionResult> Delete(int manufactureId, CancellationToken cancellationToken)
    {
        var response = await service.DeleteAsync(manufactureId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeScanApi.Application.EquipmentModalities;

namespace ResumeScanApi.Controllers;

[ApiController]
[Route("api/equipment-modalities")]
[Authorize]
public sealed class EquipmentModalitiesController(IEquipmentModalityService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await service.GetAllAsync(cancellationToken));

    [HttpGet("{equipmentModalityId:int}")]
    public async Task<IActionResult> GetById(int equipmentModalityId, CancellationToken cancellationToken)
    {
        var response = await service.GetByIdAsync(equipmentModalityId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEquipmentModalityRequest request, CancellationToken cancellationToken)
    {
        var response = await service.CreateAsync(request, cancellationToken);
        return response.Success
            ? CreatedAtAction(nameof(GetById), new { equipmentModalityId = response.Data!.EquipmentModalityId }, response)
            : Conflict(response);
    }

    [HttpPut("{equipmentModalityId:int}")]
    public async Task<IActionResult> Update(int equipmentModalityId, UpdateEquipmentModalityRequest request, CancellationToken cancellationToken)
    {
        var response = await service.UpdateAsync(equipmentModalityId, request, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpDelete("{equipmentModalityId:int}")]
    public async Task<IActionResult> Delete(int equipmentModalityId, CancellationToken cancellationToken)
    {
        var response = await service.DeleteAsync(equipmentModalityId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeScanApi.Application.Cities;

namespace ResumeScanApi.Controllers;

[ApiController]
[Route("api/cities")]
[Authorize]
public sealed class CitiesController(ICityService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int[]? stateIds, CancellationToken cancellationToken)
        => Ok(await service.GetAllAsync(cancellationToken, stateIds));

    [HttpGet("{cityId:int}")]
    public async Task<IActionResult> GetById(int cityId, CancellationToken cancellationToken)
    {
        var response = await service.GetByIdAsync(cityId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCityRequest request, CancellationToken cancellationToken)
    {
        var response = await service.CreateAsync(request, cancellationToken);
        return response.Success
            ? CreatedAtAction(nameof(GetById), new { cityId = response.Data!.CityId }, response)
            : Conflict(response);
    }

    [HttpPut("{cityId:int}")]
    public async Task<IActionResult> Update(int cityId, UpdateCityRequest request, CancellationToken cancellationToken)
    {
        var response = await service.UpdateAsync(cityId, request, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpDelete("{cityId:int}")]
    public async Task<IActionResult> Delete(int cityId, CancellationToken cancellationToken)
    {
        var response = await service.DeleteAsync(cityId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }
}

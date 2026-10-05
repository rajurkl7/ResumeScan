using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeScanApi.Application.CoverageStatuses;

namespace ResumeScanApi.Controllers;

[ApiController]
[Route("api/coverage-statuses")]
[Authorize]
public sealed class CoverageStatusesController(ICoverageStatusService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await service.GetAllAsync(cancellationToken));

    [HttpGet("{coverageStatusId:int}")]
    public async Task<IActionResult> GetById(int coverageStatusId, CancellationToken cancellationToken)
    {
        var response = await service.GetByIdAsync(coverageStatusId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCoverageStatusRequest request, CancellationToken cancellationToken)
    {
        var response = await service.CreateAsync(request, cancellationToken);
        return response.Success
            ? CreatedAtAction(nameof(GetById), new { coverageStatusId = response.Data!.CoverageStatusId }, response)
            : Conflict(response);
    }

    [HttpPut("{coverageStatusId:int}")]
    public async Task<IActionResult> Update(int coverageStatusId, UpdateCoverageStatusRequest request, CancellationToken cancellationToken)
    {
        var response = await service.UpdateAsync(coverageStatusId, request, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpDelete("{coverageStatusId:int}")]
    public async Task<IActionResult> Delete(int coverageStatusId, CancellationToken cancellationToken)
    {
        var response = await service.DeleteAsync(coverageStatusId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }
}

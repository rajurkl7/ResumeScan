using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeScanApi.Application.FieldServiceEngineers;

namespace ResumeScanApi.Controllers;

[ApiController]
[Route("api/field-service-engineers")]
[Authorize]
public sealed class FieldServiceEngineersController(IFieldServiceEngineerService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await service.GetAllAsync(cancellationToken));

    [HttpGet("{fieldServiceEngineerId:int}")]
    public async Task<IActionResult> GetById(int fieldServiceEngineerId, CancellationToken cancellationToken)
    {
        var response = await service.GetByIdAsync(fieldServiceEngineerId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpGet("{fieldServiceEngineerId:int}/documents")]
    public async Task<IActionResult> GetDocuments(int fieldServiceEngineerId, CancellationToken cancellationToken)
    {
        var documents = await service.GetDocumentsAsync(fieldServiceEngineerId, cancellationToken);
        if (documents is null)
        {
            return NotFound();
        }

        return Ok(documents.Select(document => new
        {
            document.FieldServiceEngineerDocumentId,
            document.DocumentType,
            document.OriginalFileName,
            document.ContentType,
            document.FileSizeBytes,
            document.UploadedDateTime,
            DownloadUrl = Url.Action(
                nameof(DownloadDocument),
                values: new { fieldServiceEngineerId, documentId = document.FieldServiceEngineerDocumentId })
        }));
    }

    [HttpGet("{fieldServiceEngineerId:int}/documents/{documentId:int}/download")]
    public async Task<IActionResult> DownloadDocument(
        int fieldServiceEngineerId,
        int documentId,
        CancellationToken cancellationToken)
    {
        var document = await service.GetDocumentDownloadAsync(
            fieldServiceEngineerId,
            documentId,
            cancellationToken);
        return document is null
            ? NotFound()
            : File(document.Content, document.ContentType, document.FileName);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFieldServiceEngineerRequest request, CancellationToken cancellationToken)
    {
        var response = await service.CreateAsync(request, cancellationToken);
        return response.Success
            ? CreatedAtAction(nameof(GetById), new { fieldServiceEngineerId = response.Data!.FieldServiceEngineerId }, response)
            : Conflict(response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(EngineerLoginRequest request, CancellationToken cancellationToken)
    {
        var response = await service.AuthenticateAsync(request, cancellationToken);
        return response.Success ? Ok(response) : Unauthorized(response);
    }

    [HttpPut("{fieldServiceEngineerId:int}")]
    public async Task<IActionResult> Update(int fieldServiceEngineerId, UpdateFieldServiceEngineerRequest request, CancellationToken cancellationToken)
    {
        var response = await service.UpdateAsync(fieldServiceEngineerId, request, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpDelete("{fieldServiceEngineerId:int}")]
    public async Task<IActionResult> Delete(int fieldServiceEngineerId, CancellationToken cancellationToken)
    {
        var response = await service.DeleteAsync(fieldServiceEngineerId, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }
}

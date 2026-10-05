using AutoMapper;
using ResumeScanApi.Application.Contracts;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.FieldServiceEngineers;

public sealed class FieldServiceEngineerService(
    IFieldServiceEngineerRepository repository,
    IEngineerDocumentStorage documentStorage,
    IMapper mapper) : IFieldServiceEngineerService
{
    public async Task<ApiResponse<IReadOnlyList<FieldServiceEngineerDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var engineers = await repository.GetAllAsync(cancellationToken);
        return ApiResponse<IReadOnlyList<FieldServiceEngineerDto>>.Ok(mapper.Map<IReadOnlyList<FieldServiceEngineerDto>>(engineers));
    }

    public async Task<ApiResponse<FieldServiceEngineerDto>> GetByIdAsync(int fieldServiceEngineerId, CancellationToken cancellationToken)
    {
        var engineer = await repository.GetByIdAsync(fieldServiceEngineerId, cancellationToken);
        return engineer is null
            ? ApiResponse<FieldServiceEngineerDto>.Fail("Field service engineer was not found.")
            : ApiResponse<FieldServiceEngineerDto>.Ok(mapper.Map<FieldServiceEngineerDto>(engineer));
    }

    public async Task<IReadOnlyList<EngineerDocumentDto>?> GetDocumentsAsync(
        int fieldServiceEngineerId,
        CancellationToken cancellationToken)
    {
        var engineer = await repository.GetByIdAsync(fieldServiceEngineerId, cancellationToken);
        return engineer is null
            ? null
            : mapper.Map<IReadOnlyList<EngineerDocumentDto>>(engineer.Documents);
    }

    public async Task<EngineerDocumentDownload?> GetDocumentDownloadAsync(
        int fieldServiceEngineerId,
        int documentId,
        CancellationToken cancellationToken)
    {
        var engineer = await repository.GetByIdAsync(fieldServiceEngineerId, cancellationToken);
        var document = engineer?.Documents.SingleOrDefault(
            item => item.FieldServiceEngineerDocumentId == documentId);
        if (document is null)
        {
            return null;
        }

        var content = await documentStorage.OpenReadAsync(document.StoredFilePath, cancellationToken);
        return content is null
            ? null
            : new EngineerDocumentDownload(
                content,
                document.ContentType ?? "application/octet-stream",
                Path.GetFileName(document.OriginalFileName));
    }

    public async Task<ApiResponse<FieldServiceEngineerDto>> CreateAsync(CreateFieldServiceEngineerRequest request, CancellationToken cancellationToken)
    {
        var engineer = mapper.Map<FieldServiceEngineer>(request);
        engineer.PasswordHash = PasswordHasher.Hash(request.Password);
        engineer.Documents = [];
        var storedFilePaths = new List<string>();
        FieldServiceEngineer? created;
        try
        {
            foreach (var document in request.Documents)
            {
                var storedDocument = await documentStorage.SaveAsync(document, cancellationToken);
                storedFilePaths.Add(storedDocument.StoredFilePath);
                engineer.Documents.Add(new FieldServiceEngineerDocument
                {
                    DocumentType = document.DocumentType,
                    OriginalFileName = Path.GetFileName(document.OriginalFileName),
                    StoredFilePath = storedDocument.StoredFilePath,
                    ContentType = storedDocument.ContentType,
                    FileSizeBytes = storedDocument.FileSizeBytes,
                    UploadedDateTime = DateTime.UtcNow
                });
            }

            created = await repository.CreateAsync(engineer, cancellationToken);
        }
        catch
        {
            documentStorage.Delete(storedFilePaths);
            throw;
        }

        if (created is null)
        {
            documentStorage.Delete(storedFilePaths);
            return ApiResponse<FieldServiceEngineerDto>.Fail("Field service engineer could not be created.");
        }

        return ApiResponse<FieldServiceEngineerDto>.Ok(mapper.Map<FieldServiceEngineerDto>(created), "Field service engineer created successfully.");
    }

    public async Task<ApiResponse<FieldServiceEngineerDto>> AuthenticateAsync(EngineerLoginRequest request, CancellationToken cancellationToken)
    {
        var engineer = await repository.FindByEmailAsync(request.Email, cancellationToken);
        return engineer is null || !PasswordHasher.Verify(request.Password, engineer.PasswordHash)
            ? ApiResponse<FieldServiceEngineerDto>.Fail("Invalid email or password.")
            : ApiResponse<FieldServiceEngineerDto>.Ok(mapper.Map<FieldServiceEngineerDto>(engineer));
    }

    public async Task<ApiResponse<FieldServiceEngineerDto>> UpdateAsync(int fieldServiceEngineerId, UpdateFieldServiceEngineerRequest request, CancellationToken cancellationToken)
    {
        var engineer = mapper.Map<FieldServiceEngineer>(request);
        engineer.FieldServiceEngineerId = fieldServiceEngineerId;
        engineer.PasswordHash = PasswordHasher.Hash(request.Password);
        return await repository.UpdateAsync(engineer, cancellationToken)
            ? ApiResponse<FieldServiceEngineerDto>.Ok(mapper.Map<FieldServiceEngineerDto>(engineer), "Field service engineer updated successfully.")
            : ApiResponse<FieldServiceEngineerDto>.Fail("Field service engineer was not found.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int fieldServiceEngineerId, CancellationToken cancellationToken)
    {
        return await repository.DeleteAsync(fieldServiceEngineerId, cancellationToken)
            ? ApiResponse<bool>.Ok(true, "Field service engineer deleted successfully.")
            : ApiResponse<bool>.Fail("Field service engineer was not found.");
    }
}

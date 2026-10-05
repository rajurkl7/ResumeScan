using ResumeScanApi.Application.Contracts;

namespace ResumeScanApi.Application.FieldServiceEngineers;

public interface IFieldServiceEngineerService
{
    Task<ApiResponse<IReadOnlyList<FieldServiceEngineerDto>>> GetAllAsync(CancellationToken cancellationToken);
    Task<ApiResponse<FieldServiceEngineerDto>> GetByIdAsync(int fieldServiceEngineerId, CancellationToken cancellationToken);
    Task<IReadOnlyList<EngineerDocumentDto>?> GetDocumentsAsync(int fieldServiceEngineerId, CancellationToken cancellationToken);
    Task<EngineerDocumentDownload?> GetDocumentDownloadAsync(int fieldServiceEngineerId, int documentId, CancellationToken cancellationToken);
    Task<ApiResponse<FieldServiceEngineerDto>> CreateAsync(CreateFieldServiceEngineerRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<FieldServiceEngineerDto>> AuthenticateAsync(EngineerLoginRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<FieldServiceEngineerDto>> UpdateAsync(int fieldServiceEngineerId, UpdateFieldServiceEngineerRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<bool>> DeleteAsync(int fieldServiceEngineerId, CancellationToken cancellationToken);
}

public sealed record EngineerDocumentDownload(Stream Content, string ContentType, string FileName);

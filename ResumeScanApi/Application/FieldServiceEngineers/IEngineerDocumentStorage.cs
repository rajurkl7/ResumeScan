namespace ResumeScanApi.Application.FieldServiceEngineers;

public interface IEngineerDocumentStorage
{
    Task<StoredEngineerDocument> SaveAsync(EngineerDocumentRequest document, CancellationToken cancellationToken);
    Task<Stream?> OpenReadAsync(string storedFilePath, CancellationToken cancellationToken);
    void Delete(IReadOnlyCollection<string> storedFilePaths);
}

public sealed record StoredEngineerDocument(string StoredFilePath, long FileSizeBytes, string ContentType);

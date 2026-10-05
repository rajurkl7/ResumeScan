using ResumeScanApi.Application.FieldServiceEngineers;

namespace ResumeScanApi;

public sealed class EngineerDocumentStorage(IWebHostEnvironment environment) : IEngineerDocumentStorage
{
    private const string StorageDirectoryName = "EngineerDocuments";

    public async Task<StoredEngineerDocument> SaveAsync(
        EngineerDocumentRequest document,
        CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(document.OriginalFileName).ToLowerInvariant();
        var contentType = extension switch
        {
            ".pdf" => "application/pdf",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => throw new InvalidDataException("Documents must be PDF, JPG, JPEG, or PNG files.")
        };
        var bytes = Convert.FromBase64String(document.FileContentBase64!);
        var storedFileName = $"{Guid.NewGuid():N}{extension}";
        var directoryPath = Path.Combine(environment.ContentRootPath, StorageDirectoryName);
        Directory.CreateDirectory(directoryPath);
        var absolutePath = Path.Combine(directoryPath, storedFileName);
        var temporaryPath = $"{absolutePath}.tmp";
        try
        {
            await File.WriteAllBytesAsync(temporaryPath, bytes, cancellationToken);
            File.Move(temporaryPath, absolutePath);
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }

        return new StoredEngineerDocument(
            $"{StorageDirectoryName}/{storedFileName}",
            bytes.LongLength,
            contentType);
    }

    public Task<Stream?> OpenReadAsync(string storedFilePath, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var absolutePath = GetAbsolutePath(storedFilePath);
        try
        {
            Stream stream = new FileStream(
                absolutePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 81920,
                useAsync: true);
            return Task.FromResult<Stream?>(stream);
        }
        catch (FileNotFoundException)
        {
            return Task.FromResult<Stream?>(null);
        }
        catch (DirectoryNotFoundException)
        {
            return Task.FromResult<Stream?>(null);
        }
    }

    public void Delete(IReadOnlyCollection<string> storedFilePaths)
    {
        foreach (var storedFilePath in storedFilePaths)
        {
            File.Delete(GetAbsolutePath(storedFilePath));
        }
    }

    private string GetAbsolutePath(string storedFilePath)
    {
        var expectedPrefix = $"{StorageDirectoryName}/";
        if (!storedFilePath.StartsWith(expectedPrefix, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Cannot access a document outside its storage directory.");
        }

        var fileName = storedFilePath[expectedPrefix.Length..];
        if (string.IsNullOrWhiteSpace(fileName)
            || fileName != Path.GetFileName(fileName)
            || !Guid.TryParseExact(Path.GetFileNameWithoutExtension(fileName), "N", out _)
            || Path.GetExtension(fileName).ToLowerInvariant() is not (".pdf" or ".jpg" or ".jpeg" or ".png"))
        {
            throw new InvalidOperationException("The stored document path is invalid.");
        }

        return Path.Combine(environment.ContentRootPath, StorageDirectoryName, fileName);
    }
}

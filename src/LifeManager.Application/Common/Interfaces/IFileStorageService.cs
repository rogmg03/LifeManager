namespace LifeManager.Application.Common.Interfaces;

public interface IFileStorageService
{
    /// <summary>Saves a file and returns the storage path (relative key).</summary>
    Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct = default);

    /// <summary>Deletes a file by its storage path.</summary>
    Task DeleteFileAsync(string storagePath, CancellationToken ct = default);

    /// <summary>Opens a read stream for a stored file.</summary>
    Task<Stream> GetFileStreamAsync(string storagePath, CancellationToken ct = default);
}

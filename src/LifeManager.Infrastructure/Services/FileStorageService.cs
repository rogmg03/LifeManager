using LifeManager.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LifeManager.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public FileStorageService(IConfiguration configuration)
    {
        _basePath = configuration["FileStorage:BasePath"]
            ?? Path.Combine(Path.GetTempPath(), "lifemanager-files");
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> SaveFileAsync(
        Stream fileStream, string fileName, string contentType,
        CancellationToken ct = default)
    {
        var safeFileName = Path.GetFileName(fileName);
        var uniqueName = $"{Guid.NewGuid()}_{safeFileName}";
        var filePath = Path.Combine(_basePath, uniqueName);

        await using var fs = File.Create(filePath);
        await fileStream.CopyToAsync(fs, ct);

        return uniqueName;
    }

    public Task DeleteFileAsync(string storagePath, CancellationToken ct = default)
    {
        var filePath = Path.Combine(_basePath, storagePath);
        if (File.Exists(filePath))
            File.Delete(filePath);
        return Task.CompletedTask;
    }

    public Task<Stream> GetFileStreamAsync(string storagePath, CancellationToken ct = default)
    {
        var filePath = Path.Combine(_basePath, storagePath);
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {storagePath}");
        Stream stream = File.OpenRead(filePath);
        return Task.FromResult(stream);
    }
}

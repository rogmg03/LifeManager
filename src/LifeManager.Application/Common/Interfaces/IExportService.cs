using LifeManager.Application.Features.Export.DTOs;

namespace LifeManager.Application.Common.Interfaces;

public interface IExportService
{
    Task<ExportDataDto> GetAllDataAsync(Guid userId, CancellationToken ct = default);
    Task<string> GetCsvAsync(Guid userId, string entity, CancellationToken ct = default);
}

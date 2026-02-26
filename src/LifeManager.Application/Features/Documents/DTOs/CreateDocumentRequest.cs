namespace LifeManager.Application.Features.Documents.DTOs;

public record CreateDocumentRequest(
    string Title,
    string? Content);

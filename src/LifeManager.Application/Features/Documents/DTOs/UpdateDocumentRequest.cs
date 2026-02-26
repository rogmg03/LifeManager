namespace LifeManager.Application.Features.Documents.DTOs;

public record UpdateDocumentRequest(
    string Title,
    string? Content);

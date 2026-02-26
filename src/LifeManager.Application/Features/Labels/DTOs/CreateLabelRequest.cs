namespace LifeManager.Application.Features.Labels.DTOs;

public record CreateLabelRequest(
    string Name,
    string Color = "#6366F1");

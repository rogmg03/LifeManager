using LifeManager.Application.Features.Attachments.Commands;
using LifeManager.Application.Features.Attachments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class AttachmentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public AttachmentsController(IMediator mediator) => _mediator = mediator;

    // POST /api/documents/{id}/attachments
    [HttpPost("api/documents/{id:guid}/attachments")]
    public async Task<IActionResult> UploadToDocument(
        Guid id,
        IFormFile file,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new UploadAttachmentCommand(
            DocumentId: id,
            TaskId: null,
            FileName: file.FileName,
            ContentType: file.ContentType,
            FileSizeBytes: file.Length,
            FileStream: file.OpenReadStream()), ct);
        return StatusCode(201, result);
    }

    // POST /api/tasks/{id}/attachments
    [HttpPost("api/tasks/{id:guid}/attachments")]
    public async Task<IActionResult> UploadToTask(
        Guid id,
        IFormFile file,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new UploadAttachmentCommand(
            DocumentId: null,
            TaskId: id,
            FileName: file.FileName,
            ContentType: file.ContentType,
            FileSizeBytes: file.Length,
            FileStream: file.OpenReadStream()), ct);
        return StatusCode(201, result);
    }

    // GET /api/attachments/{id}/download
    [HttpGet("api/attachments/{id:guid}/download")]
    public async Task<IActionResult> Download(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DownloadAttachmentQuery(id), ct);
        return File(result.FileStream, result.ContentType, result.FileName);
    }

    // DELETE /api/attachments/{id}
    [HttpDelete("api/attachments/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteAttachmentCommand(id), ct);
        return NoContent();
    }
}

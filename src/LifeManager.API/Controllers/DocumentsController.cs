using LifeManager.Application.Features.Documents.Commands;
using LifeManager.Application.Features.Documents.DTOs;
using LifeManager.Application.Features.Documents.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class DocumentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public DocumentsController(IMediator mediator) => _mediator = mediator;

    // GET /api/projects/{projectId}/documents
    [HttpGet("api/projects/{projectId:guid}/documents")]
    public async Task<IActionResult> GetByProject(Guid projectId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectDocumentsQuery(projectId), ct));

    // GET /api/documents/{id}
    [HttpGet("api/documents/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDocumentByIdQuery(id), ct));

    // GET /api/documents/search?q=
    [HttpGet("api/documents/search")]
    public async Task<IActionResult> Search([FromQuery] string q, CancellationToken ct)
        => Ok(await _mediator.Send(new SearchDocumentsQuery(q ?? string.Empty), ct));

    // POST /api/projects/{projectId}/documents
    [HttpPost("api/projects/{projectId:guid}/documents")]
    public async Task<IActionResult> Create(
        Guid projectId,
        [FromBody] CreateDocumentRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(
            new CreateDocumentCommand(projectId, request.Title, request.Content), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // PUT /api/documents/{id}
    [HttpPut("api/documents/{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateDocumentRequest request,
        CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateDocumentCommand(id, request.Title, request.Content), ct));

    // DELETE /api/documents/{id}
    [HttpDelete("api/documents/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteDocumentCommand(id), ct);
        return NoContent();
    }
}

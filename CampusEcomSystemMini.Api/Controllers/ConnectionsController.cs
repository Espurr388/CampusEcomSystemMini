using CampusEcomSystemMini.Application.Connections.AcceptRequest;
using CampusEcomSystemMini.Application.Connections.GetRequests;
using CampusEcomSystemMini.Application.Connections.RejectRequest;
using CampusEcomSystemMini.Application.Connections.SendRequest;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusEcomSystemMini.Api.Controllers;

[ApiController]
[Route("api/connections/requests")]
[Authorize]
public class ConnectionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConnectionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Send(
        SendRequestCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command,
            cancellationToken);

        return StatusCode(201, result);
    }

    [HttpGet]
    public async Task<ActionResult<GetRequestsResponse>> GetRequests(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetRequestsQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}/accept")]
    public async Task<ActionResult<AcceptRequestResponse>> Accept(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new AcceptRequestCommand(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}/reject")]
    public async Task<ActionResult<RejectRequestResponse>> Reject(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new RejectRequestCommand(id),
            cancellationToken);

        return Ok(result);
    }
}
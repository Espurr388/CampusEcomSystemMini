using CampusEcomSystemMini.Application.Users.Profile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusEcomSystemMini.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("me")]
    public async Task<ActionResult<GetProfileResponse>> GetProfile(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetProfileQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("me")]
    public async Task<ActionResult<UpdateProfileResponse>> UpdateProfile(
        UpdateProfileCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("me/avatar")]
    public async Task<ActionResult<UpdateAvatarResponse>> UpdateAvatar(
        UpdateAvatarCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("me/password")]
    public async Task<ActionResult<ChangePasswordResponse>> ChangePassword(
        ChangePasswordCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command,
            cancellationToken);

        return Ok(result);
    }
}

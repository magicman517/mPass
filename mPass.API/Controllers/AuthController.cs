using MediatR;
using Microsoft.AspNetCore.Mvc;
using mPass.Application.Auth.Dtos;
using mPass.Application.Users.Dtos;
using mPass.Application.Users.Queries;

namespace mPass.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("Start")]
    [EndpointSummary("Start authentication process")]
    [ProducesResponseType(typeof(GetUserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> StartAuth([FromBody] StartAuthRequest request, CancellationToken cancellationToken)
    {
        var resultEmail = await mediator.Send(new GetUserQuery { Email = request.Identifier },
            cancellationToken);
        if (resultEmail.IsSuccess)
        {
            return Ok(resultEmail.Value);
        }
        
        var resultUsername = await mediator.Send(new GetUserQuery { Username = request.Identifier },
            cancellationToken);
        if (resultUsername.IsSuccess)
        {
            return Ok(resultUsername.Value);
        }

        return BadRequest(new { errors = resultUsername.Errors });
    }
}
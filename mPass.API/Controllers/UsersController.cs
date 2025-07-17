using MediatR;
using Microsoft.AspNetCore.Mvc;
using mPass.Application.Users.Commands;

namespace mPass.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Create user")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateUser(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess
            ? Created()
            : BadRequest(new { errors = result.Errors });
    }
}
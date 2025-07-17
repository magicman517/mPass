using Microsoft.AspNetCore.Mvc;

namespace mPass.API.Controllers;

[ApiController]
[Route("/")]
public class RootController : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Check server health")]
    public IActionResult Get()
    {
        return Ok();
    }
}
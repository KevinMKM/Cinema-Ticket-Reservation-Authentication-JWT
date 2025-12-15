using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/screenings")]
public class ScreeningsController : ControllerBase
{
    [Authorize(Policy = "CreateScreening")]
    [HttpPost]
    public IActionResult Create() => Ok();
}
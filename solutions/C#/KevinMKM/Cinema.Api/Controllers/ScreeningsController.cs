using Cinema.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")] // Role-based
public class ScreeningsController : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = "RequireScope:screening:create")] // Scope-based
    public IActionResult CreateScreening([FromBody] CreateScreeningRequest request)
    {
        return Ok(new { id = Guid.NewGuid(), message = "Screening created" });
    }
}
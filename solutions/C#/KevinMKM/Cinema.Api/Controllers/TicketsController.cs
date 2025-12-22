using Cinema.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Base authentication
public class TicketsController : ControllerBase
{
    // ? SCOPE-BASED AUTHORIZATION - Critical requirement
    [HttpPost("reserve")]
    [Authorize(Policy = "RequireScope:ticket:reserve")]
    public IActionResult ReserveTicket([FromBody] ReserveTicketRequest request)
    {
        return Ok(new ReserveTicketResponse(Guid.NewGuid(), "Ticket reserved successfully"));
    }
}
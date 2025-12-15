using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketsController : ControllerBase
{
    [Authorize(Policy = "ReserveTicket")]
    [HttpPost("reserve")]
    public IActionResult Reserve() => Ok(new { status = "Reserved" });
}
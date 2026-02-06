using CustomerSupport.API.DTOs;
using CustomerSupport.API.Models;
using CustomerSupport.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomerSupport.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TicketDto>>> GetTickets()
        {
            var userId = GetCurrentUserId();
            var isAdmin = IsCurrentUserAdmin();

            var tickets = await _ticketService.GetTicketsAsync(userId, isAdmin);
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetTicket(int id)
        {
            var userId = GetCurrentUserId();
            var isAdmin = IsCurrentUserAdmin();

            var ticket = await _ticketService.GetTicketByIdAsync(id, userId, isAdmin);
            
            if (ticket == null)
            {
                return NotFound(new { message = "Ticket not found or access denied" });
            }

            return Ok(ticket);
        }

        [HttpPost]
        public async Task<ActionResult<TicketDto>> CreateTicket([FromBody] CreateTicketRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var ticket = await _ticketService.CreateTicketAsync(request, userId);

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTicket(int id, [FromBody] UpdateTicketRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var isAdmin = IsCurrentUserAdmin();

            var success = await _ticketService.UpdateTicketAsync(id, request, userId, isAdmin);
            
            if (!success)
            {
                return NotFound(new { message = "Ticket not found or access denied" });
            }

            return Ok(new { message = "Ticket updated successfully" });
        }

        [HttpPost("{id}/comments")]
        public async Task<ActionResult> AddComment(int id, [FromBody] AddCommentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var isAdmin = IsCurrentUserAdmin();

            var success = await _ticketService.AddCommentAsync(id, request.Comment, userId, request.IsInternal && isAdmin);
            
            if (!success)
            {
                return NotFound(new { message = "Ticket not found" });
            }

            return Ok(new { message = "Comment added successfully" });
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? "0");
        }

        private bool IsCurrentUserAdmin()
        {
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            return roleClaim?.Value == UserRole.Admin.ToString();
        }
    }

    public class AddCommentRequest
    {
        public string Comment { get; set; } = string.Empty;
        public bool IsInternal { get; set; } = false;
    }
}
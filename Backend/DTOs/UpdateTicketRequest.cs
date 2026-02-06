using CustomerSupport.API.Models;

namespace CustomerSupport.API.DTOs
{
    public class UpdateTicketRequest
    {
        public int? AssignedToId { get; set; }
        public TicketStatus? Status { get; set; }
        public string? Comment { get; set; }
        public bool IsInternalComment { get; set; } = false;
    }
}
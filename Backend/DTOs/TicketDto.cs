using CustomerSupport.API.Models;

namespace CustomerSupport.API.DTOs
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string TicketNumber { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketPriority Priority { get; set; }
        public TicketStatus Status { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public int? AssignedToId { get; set; }
        public string? AssignedToName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<TicketCommentDto> Comments { get; set; } = new List<TicketCommentDto>();
        public List<TicketStatusHistoryDto> StatusHistory { get; set; } = new List<TicketStatusHistoryDto>();
    }
}
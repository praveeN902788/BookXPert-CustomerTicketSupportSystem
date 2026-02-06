namespace CustomerSupport.Desktop.Models
{
    public class Ticket
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
        public List<TicketComment> Comments { get; set; } = new List<TicketComment>();
        public List<TicketStatusHistory> StatusHistory { get; set; } = new List<TicketStatusHistory>();
    }

    public enum TicketPriority
    {
        Low,
        Medium,
        High
    }

    public enum TicketStatus
    {
        Open,
        InProgress,
        Closed
    }
}
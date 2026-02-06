namespace CustomerSupport.Desktop.Models
{
    public class TicketStatusHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public TicketStatus? OldStatus { get; set; }
        public TicketStatus NewStatus { get; set; }
        public int ChangedById { get; set; }
        public string ChangedByName { get; set; } = string.Empty;
        public DateTime ChangeDate { get; set; }
        public string? Comments { get; set; }
    }
}
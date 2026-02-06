namespace CustomerSupport.Desktop.Models
{
    public class CreateTicketRequest
    {
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketPriority Priority { get; set; }
    }
}
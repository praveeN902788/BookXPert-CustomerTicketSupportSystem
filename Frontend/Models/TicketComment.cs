namespace CustomerSupport.Desktop.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public bool IsInternal { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
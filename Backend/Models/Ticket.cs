using System.ComponentModel.DataAnnotations;

namespace CustomerSupport.API.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(20)]
        public string TicketNumber { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public TicketPriority Priority { get; set; } = TicketPriority.Medium;
        
        [Required]
        public TicketStatus Status { get; set; } = TicketStatus.Open;
        
        [Required]
        public int CreatedById { get; set; }
        
        public int? AssignedToId { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
       
        public virtual User CreatedBy { get; set; } = null!;
        public virtual User? AssignedTo { get; set; }
        public virtual ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
        public virtual ICollection<TicketStatusHistory> StatusHistory { get; set; } = new List<TicketStatusHistory>();
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
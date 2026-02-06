using System.ComponentModel.DataAnnotations;

namespace CustomerSupport.API.Models
{
    public class TicketStatusHistory
    {
        public int Id { get; set; }
        
        [Required]
        public int TicketId { get; set; }
        
        public TicketStatus? OldStatus { get; set; }
        
        [Required]
        public TicketStatus NewStatus { get; set; }
        
        [Required]
        public int ChangedById { get; set; }
        
        public DateTime ChangeDate { get; set; } = DateTime.UtcNow;
        
        public string? Comments { get; set; }
        
        public virtual Ticket Ticket { get; set; } = null!;
        public virtual User ChangedBy { get; set; } = null!;
    }
}
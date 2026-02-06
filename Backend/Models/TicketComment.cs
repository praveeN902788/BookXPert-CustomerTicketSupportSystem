using System.ComponentModel.DataAnnotations;

namespace CustomerSupport.API.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        
        [Required]
        public int TicketId { get; set; }
        
        [Required]
        public string CommentText { get; set; } = string.Empty;
        
        public bool IsInternal { get; set; } = false;
        
        [Required]
        public int CreatedById { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public virtual Ticket Ticket { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
    }
}
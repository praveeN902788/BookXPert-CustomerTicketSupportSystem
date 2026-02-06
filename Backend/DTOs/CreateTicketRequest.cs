using CustomerSupport.API.Models;
using System.ComponentModel.DataAnnotations;

namespace CustomerSupport.API.DTOs
{
    public class CreateTicketRequest
    {
        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public TicketPriority Priority { get; set; }
    }
}
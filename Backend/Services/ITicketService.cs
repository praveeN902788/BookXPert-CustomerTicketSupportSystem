using CustomerSupport.API.DTOs;

namespace CustomerSupport.API.Services
{
    public interface ITicketService
    {
        Task<List<TicketDto>> GetTicketsAsync(int userId, bool isAdmin);
        Task<TicketDto?> GetTicketByIdAsync(int ticketId, int userId, bool isAdmin);
        Task<TicketDto> CreateTicketAsync(CreateTicketRequest request, int userId);
        Task<bool> UpdateTicketAsync(int ticketId, UpdateTicketRequest request, int userId, bool isAdmin);
        Task<bool> AddCommentAsync(int ticketId, string comment, int userId, bool isInternal = false);
    }
}
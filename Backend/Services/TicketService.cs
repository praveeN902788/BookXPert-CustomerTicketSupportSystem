using CustomerSupport.API.Data;
using CustomerSupport.API.DTOs;
using CustomerSupport.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerSupport.API.Services
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext _context;

        public TicketService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TicketDto>> GetTicketsAsync(int userId, bool isAdmin)
        {
            var query = _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(t => t.CreatedById == userId);
            }

            var tickets = await query
                .OrderByDescending(t => t.CreatedDate)
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    TicketNumber = t.TicketNumber,
                    Subject = t.Subject,
                    Description = t.Description,
                    Priority = t.Priority,
                    Status = t.Status,
                    CreatedById = t.CreatedById,
                    CreatedByName = t.CreatedBy.FullName,
                    AssignedToId = t.AssignedToId,
                    AssignedToName = t.AssignedTo != null ? t.AssignedTo.FullName : null,
                    CreatedDate = t.CreatedDate,
                    UpdatedDate = t.UpdatedDate
                })
                .ToListAsync();

            return tickets;
        }

        public async Task<TicketDto?> GetTicketByIdAsync(int ticketId, int userId, bool isAdmin)
        {
            var query = _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.CreatedBy)
                .Include(t => t.StatusHistory)
                    .ThenInclude(h => h.ChangedBy)
                .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(t => t.CreatedById == userId);
            }

            var ticket = await query.FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket == null) return null;

            return new TicketDto
            {
                Id = ticket.Id,
                TicketNumber = ticket.TicketNumber,
                Subject = ticket.Subject,
                Description = ticket.Description,
                Priority = ticket.Priority,
                Status = ticket.Status,
                CreatedById = ticket.CreatedById,
                CreatedByName = ticket.CreatedBy.FullName,
                AssignedToId = ticket.AssignedToId,
                AssignedToName = ticket.AssignedTo?.FullName,
                CreatedDate = ticket.CreatedDate,
                UpdatedDate = ticket.UpdatedDate,
                Comments = ticket.Comments
                    .Where(c => isAdmin || !c.IsInternal)
                    .Select(c => new TicketCommentDto
                    {
                        Id = c.Id,
                        TicketId = c.TicketId,
                        CommentText = c.CommentText,
                        IsInternal = c.IsInternal,
                        CreatedById = c.CreatedById,
                        CreatedByName = c.CreatedBy.FullName,
                        CreatedDate = c.CreatedDate
                    })
                    .OrderBy(c => c.CreatedDate)
                    .ToList(),
                StatusHistory = ticket.StatusHistory
                    .Select(h => new TicketStatusHistoryDto
                    {
                        Id = h.Id,
                        TicketId = h.TicketId,
                        OldStatus = h.OldStatus,
                        NewStatus = h.NewStatus,
                        ChangedById = h.ChangedById,
                        ChangedByName = h.ChangedBy.FullName,
                        ChangeDate = h.ChangeDate,
                        Comments = h.Comments
                    })
                    .OrderBy(h => h.ChangeDate)
                    .ToList()
            };
        }

        public async Task<TicketDto> CreateTicketAsync(CreateTicketRequest request, int userId)
        {
            var ticket = new Ticket
            {
                TicketNumber = "TKT" + DateTime.UtcNow.Ticks,
                Subject = request.Subject,
                Description = request.Description,
                Priority = request.Priority,
                Status = TicketStatus.Open,
                CreatedById = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            try
            {
                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }


            // Add initial status history
            var statusHistory = new TicketStatusHistory
            {
                TicketId = ticket.Id,
                OldStatus = null,
                NewStatus = TicketStatus.Open,
                ChangedById = userId,
                ChangeDate = DateTime.UtcNow,
                Comments = "Ticket created"
            };

            _context.TicketStatusHistory.Add(statusHistory);
            await _context.SaveChangesAsync();

            // Load the created ticket with related data
            var createdTicket = await _context.Tickets
                .Include(t => t.CreatedBy)
                .FirstAsync(t => t.Id == ticket.Id);

            return new TicketDto
            {
                Id = createdTicket.Id,
                TicketNumber = createdTicket.TicketNumber,
                Subject = createdTicket.Subject,
                Description = createdTicket.Description,
                Priority = createdTicket.Priority,
                Status = createdTicket.Status,
                CreatedById = createdTicket.CreatedById,
                CreatedByName = createdTicket.CreatedBy.FullName,
                AssignedToId = createdTicket.AssignedToId,
                AssignedToName = null,
                CreatedDate = createdTicket.CreatedDate,
                UpdatedDate = createdTicket.UpdatedDate
            };
        }

        public async Task<bool> UpdateTicketAsync(int ticketId, UpdateTicketRequest request, int userId, bool isAdmin)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return false;
            if (!isAdmin && ticket.CreatedById != userId) return false;
            if (ticket.Status == TicketStatus.Closed && !isAdmin) return false;

            bool hasChanges = false;
            if (isAdmin && request.AssignedToId.HasValue && ticket.AssignedToId != request.AssignedToId)
            {
                ticket.AssignedToId = request.AssignedToId;
                hasChanges = true;
            }
            if (isAdmin && request.Status.HasValue && ticket.Status != request.Status)
            {
                var oldStatus = ticket.Status;
                ticket.Status = request.Status.Value;
                ticket.UpdatedDate = DateTime.UtcNow;
                hasChanges = true;
                var statusHistory = new TicketStatusHistory
                {
                    TicketId = ticketId,
                    OldStatus = oldStatus,
                    NewStatus = request.Status.Value,
                    ChangedById = userId,
                    ChangeDate = DateTime.UtcNow,
                    Comments = request.Comment
                };

                _context.TicketStatusHistory.Add(statusHistory);
            }
            if (!string.IsNullOrWhiteSpace(request.Comment))
            {
                var comment = new TicketComment
                {
                    TicketId = ticketId,
                    CommentText = request.Comment,
                    IsInternal = request.IsInternalComment && isAdmin,
                    CreatedById = userId,
                    CreatedDate = DateTime.UtcNow
                };

                _context.TicketComments.Add(comment);
                hasChanges = true;
            }

            if (hasChanges)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    throw;
                }
            }

            return true;
        }

        public async Task<bool> AddCommentAsync(int ticketId, string comment, int userId, bool isInternal = false)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return false;

            var ticketComment = new TicketComment
            {
                TicketId = ticketId,
                CommentText = comment,
                IsInternal = isInternal,
                CreatedById = userId,
                CreatedDate = DateTime.UtcNow
            };

            _context.TicketComments.Add(ticketComment);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
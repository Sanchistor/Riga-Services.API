using Microsoft.EntityFrameworkCore;
using riga.services.DbContext;
using riga.services.Models;
using riga.services.riga.services.ticket_manager.DTO.UserTickets;
using riga.services.riga.services.ticket_manager.IRepositories;

namespace riga.services.riga.services.ticket_manager.Repositories;

public class TicketRepository: ITicketRepository
{
    private readonly ApiDbContext _context;

    public TicketRepository(ApiDbContext context)
    {
        _context = context;
    }
    public async Task<TicketsInfo?> get_ticket_data(Guid ticketId, CancellationToken cancellationToken)
    {
        return await _context.TicketsInfo.FindAsync(new object[] { ticketId }, cancellationToken);
    }

    public async Task<bool> save_user_tickets(UserTickets userTickets, CancellationToken cancellationToken)
    {
        _context.UserTickets.Add(userTickets);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    public async Task<UserTicketWithType> get_user_unregistered_ticket(Guid userId, Guid UserTicketId, CancellationToken cancellationToken)
    {
        return await _context.UserTickets
            .Where(ticket => ticket.UserId == userId && ticket.BusDataId == null && ticket.Id == UserTicketId)
            .Join(
                _context.TicketsInfo,
                userTicket => userTicket.TicketsId,
                ticketInfo => ticketInfo.Id,
                (userTicket, ticketInfo) => new UserTicketWithType
                {
                    UserTicket = userTicket,
                    TicketType = ticketInfo.TicketType
                }
            )
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<BusData> get_bus_data_by_code(int BusCode, CancellationToken cancellationToken)
    {
        return await _context.BusData.Where(bus => bus.BusCode == BusCode).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> update_user_ticket(UpdateUserTicketsDto updateUserTicketsDto, CancellationToken cancellationToken)
    {

        var userTicket = await _context.UserTickets
            .FirstOrDefaultAsync(ticket => ticket.Id == updateUserTicketsDto.UserTicketId, cancellationToken);

        if (userTicket == null)
        {
            return false;
        }

        userTicket.StartDate = updateUserTicketsDto.StartDate;
        userTicket.DueTime = updateUserTicketsDto.DueTime;

        if (updateUserTicketsDto.busData != null)
        {
            userTicket.BusDataId = updateUserTicketsDto.busData.Id; 
            userTicket.BusData = updateUserTicketsDto.busData; 
        }

        _context.UserTickets.Update(userTicket);
        var changes = await _context.SaveChangesAsync(cancellationToken);

        return changes > 0; 
    }
}
using Microsoft.EntityFrameworkCore;
using riga.services.DbContext;
using riga.services.Models;
using riga.services.riga.services.ticket_manager.DTO.UserTickets;
using riga.services.riga.services.ticket_manager.IRepositories;
using riga.services.riga.services.ticket_manager.Responses.UserTickets;

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

    public async Task<bool> pay_ticket(double TicketPrice, Guid UserId, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Where(user => user.Id == UserId && user.Balance >= TicketPrice).FirstOrDefaultAsync( cancellationToken);
        if (user == null)
        {
            return false;
        }
        user.Balance -= TicketPrice;
        _context.Users.Update(user);
        var changes = await _context.SaveChangesAsync(cancellationToken);

        return changes > 0; 
    }

    public async Task<List<UnregisteredTicketsResponse>> get_unregistered_tickets(Guid UserId, CancellationToken cancellationToken)
    {
        var tickets = await _context.UserTickets
            .Where(ticket => ticket.UserId == UserId && ticket.BusDataId == null && ticket.StartDate == null && ticket.DueTime == null)
            .Join(
                _context.TicketsInfo,         
                userTicket => userTicket.TicketsId,  
                ticketInfo => ticketInfo.Id,         
                (userTicket, ticketInfo) => new UnregisteredTicketsResponse 
                {
                    UserTicketId = userTicket.Id,
                    TicketId = userTicket.TicketsId,
                    Name = ticketInfo.TicketType.ToString()  
                }
            )
            .ToListAsync(cancellationToken);

        return tickets;

    }

    public async Task<bool> validate_tickets(Guid UserId, CancellationToken cancellationToken)
    {
        var currentTime = DateTime.UtcNow;
        
        var expiredTicketsCount = await _context.UserTickets
            .Where(ticket => ticket.UserId == UserId 
                             && ticket.StartDate != null 
                             && ticket.DueTime != null 
                             && ticket.DueTime <= currentTime 
                             && ticket.Valid)  
            .ExecuteUpdateAsync(ticket => ticket.SetProperty(t => t.Valid, false), cancellationToken);
        
        return expiredTicketsCount > 0;
    }

    public async Task<List<RigisteredTicketResponse>> get_valid_tickets(Guid UserId, CancellationToken cancellationToken)
    {
        var currentTime = DateTime.UtcNow;
    
        var validTickets = await _context.UserTickets
            .Where(ticket => ticket.UserId == UserId && ticket.Valid) 
            .Join(
                _context.TicketsInfo, 
                userTicket => userTicket.TicketsId,
                ticketInfo => ticketInfo.Id,
                (userTicket, ticketInfo) => new { userTicket, ticketInfo }
            )
            .Join(
                _context.BusData,
                combined => combined.userTicket.BusDataId,
                busData => busData.Id,
                (combined, busData) => new RigisteredTicketResponse
                {
                    UserId = combined.userTicket.UserId,
                    BusCode = busData.BusCode,
                    TicketType = combined.ticketInfo.TicketType.ToString(),
                    TicketValid = combined.userTicket.Valid,
                    ValidUntil = combined.userTicket.DueTime,
                    CurrentTime = currentTime,
                    Success = true,
                    Message = "Active valid ticket"
                }
            )
            .ToListAsync(cancellationToken);

        return validTickets;
    }

    public async Task<List<HistoryOfTripsResponse>> get_history_of_trips(Guid UserId, CancellationToken cancellationToken)
    {
        var historyOfTrips = await _context.UserTickets
            .Where(ticket => ticket.UserId == UserId && ticket.StartDate != null && !ticket.Valid)  
            .Join(
                _context.TicketsInfo, 
                userTicket => userTicket.TicketsId,
                ticketInfo => ticketInfo.Id,
                (userTicket, ticketInfo) => new { userTicket, ticketInfo }
            )
            .Join(
                _context.BusData,
                combined => combined.userTicket.BusDataId,
                busData => busData.Id,
                (combined, busData) => new HistoryOfTripsResponse
                {
                    BusCode = busData.BusCode,
                    TicketType = combined.ticketInfo.TicketType.ToString(),
                    DateOfIssue = combined.userTicket.StartDate.Value  
                }
            )
            .OrderByDescending(trip => trip.DateOfIssue)  
            .ToListAsync(cancellationToken);

        return historyOfTrips;
    }
}
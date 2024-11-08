using riga.services.Models;
using riga.services.riga.services.ticket_manager.DTO.UserTickets;
using riga.services.riga.services.ticket_manager.Responses.UserTickets;

namespace riga.services.riga.services.ticket_manager.IRepositories;

public interface ITicketRepository
{
    public Task<TicketsInfo?> get_ticket_data(Guid ticketId, CancellationToken cancellationToken);
    public Task<bool> save_user_tickets(UserTickets userTickets, CancellationToken cancellationToken);
    public Task<UserTicketWithType> get_user_unregistered_ticket(Guid userId, Guid UserTicketId, CancellationToken cancellationToken);
    public Task<BusData> get_bus_data_by_code(int BusCode, CancellationToken cancellationToken);

    public Task<bool> update_user_ticket(UpdateUserTicketsDto updateUserTicketsDto,
        CancellationToken cancellationToken);

    public Task<bool> pay_ticket(double TicketPrice, Guid UserId, CancellationToken cancellationToken);

    public Task<List<UnregisteredTicketsResponse>> get_unregistered_tickets(Guid UserId, CancellationToken cancellationToken);
    public Task<bool> validate_tickets(Guid UserId, CancellationToken cancellationToken);
    
    public Task<List<RigisteredTicketResponse>> get_valid_tickets(Guid UserId, CancellationToken cancellationToken);
    public Task<List<HistoryOfTripsResponse>> get_history_of_trips(Guid UserId, CancellationToken cancellationToken);
}
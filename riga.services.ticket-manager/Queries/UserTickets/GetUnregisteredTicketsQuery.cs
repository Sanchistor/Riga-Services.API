using MediatR;
using riga.services.riga.services.authentication.Services.Guard;
using riga.services.riga.services.ticket_manager.IRepositories;
using riga.services.riga.services.ticket_manager.Responses.UserTickets;

namespace riga.services.riga.services.ticket_manager.Queries.UserTickets;

public class GetUnregisteredTicketsQuery: IRequest<List<UnregisteredTicketsResponse>>
{
    
}

public class GetUnregisteredTicketsQueryHandler: IRequestHandler<GetUnregisteredTicketsQuery, List<UnregisteredTicketsResponse>>
{
    private readonly AuthGuard _authGuard;
    private readonly ITicketRepository _ticketRepository;

    public GetUnregisteredTicketsQueryHandler(ITicketRepository ticketRepository, AuthGuard authGuard)
    {
        _authGuard = authGuard;
        _ticketRepository = ticketRepository;
    }

    public async Task<List<UnregisteredTicketsResponse>> Handle(GetUnregisteredTicketsQuery request, CancellationToken cancellationToken)
    {
        return await this.GetUnregisteredTicketsAsync(cancellationToken);
    }

    private async Task<List<UnregisteredTicketsResponse>> GetUnregisteredTicketsAsync(
        CancellationToken cancellationToken)
    {
        var userGuid = _authGuard.GetUserId();
        return await _ticketRepository.get_unregistered_tickets((Guid)userGuid, cancellationToken);
    }
}
using MediatR;
using riga.services.riga.services.authentication.Services.Guard;
using riga.services.riga.services.ticket_manager.IRepositories;
using riga.services.riga.services.ticket_manager.Responses.UserTickets;

namespace riga.services.riga.services.ticket_manager.Queries.UserTickets;

public class GetValidTicketsQuery: IRequest<List<RigisteredTicketResponse>>
{
    
}

public class GetValidTicketsQueryHandler: IRequestHandler<GetValidTicketsQuery, List<RigisteredTicketResponse>>
{
    private readonly AuthGuard _authGuard;
    private readonly ITicketRepository _ticketRepository;

    public GetValidTicketsQueryHandler(ITicketRepository ticketRepository, AuthGuard authGuard)
    {
        _authGuard = authGuard;
        _ticketRepository = ticketRepository;
    }
    
    public async Task<List<RigisteredTicketResponse>> Handle(GetValidTicketsQuery request, CancellationToken cancellationToken)
    {
        var userGuid = _authGuard.GetUserId();
        var updatedTickets = await _ticketRepository.validate_tickets((Guid)userGuid, cancellationToken);
        return await _ticketRepository.get_valid_tickets((Guid)userGuid, cancellationToken);
    }
}
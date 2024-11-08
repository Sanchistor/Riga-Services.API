using MediatR;
using riga.services.riga.services.authentication.Services.Guard;
using riga.services.riga.services.ticket_manager.IRepositories;
using riga.services.riga.services.ticket_manager.Responses.UserTickets;

namespace riga.services.riga.services.ticket_manager.Queries.UserTickets;

public class GetHistoryOfTripsQuery: IRequest<List<HistoryOfTripsResponse>>
{
    
}

public class GetHistoryOfTripsQueryHandler: IRequestHandler<GetHistoryOfTripsQuery, List<HistoryOfTripsResponse>>
{
    private readonly AuthGuard _authGuard;
    private readonly ITicketRepository _ticketRepository;

    public GetHistoryOfTripsQueryHandler(ITicketRepository ticketRepository, AuthGuard authGuard)
    {
        _authGuard = authGuard;
        _ticketRepository = ticketRepository;
    }
    
    public async Task<List<HistoryOfTripsResponse>> Handle(GetHistoryOfTripsQuery request, CancellationToken cancellationToken)
    {
        return await this.GetHistoryAsync(cancellationToken);
    }

    private async Task<List<HistoryOfTripsResponse>> GetHistoryAsync(CancellationToken cancellationToken)
    {
        var userGuid = (Guid)_authGuard.GetUserId();
        return await _ticketRepository.get_history_of_trips(userGuid, cancellationToken);
    }
}
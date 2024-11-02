using MediatR;
using Microsoft.EntityFrameworkCore;
using riga.services.DbContext;
using riga.services.riga.services.ticket_manager.Responses;

namespace riga.services.riga.services.ticket_manager.Queries;

public class GetAllTicketsQuery:IRequest<List<AllTicketsTypeResponse>>
{

}

public class GetAllTicketsQueryHandler : IRequestHandler<GetAllTicketsQuery, List<AllTicketsTypeResponse>>
{
    private readonly ApiDbContext _context;

    public GetAllTicketsQueryHandler(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<List<AllTicketsTypeResponse>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _context.TicketsInfo.ToListAsync(cancellationToken);

        var response = tickets.Select(ticket => new AllTicketsTypeResponse
        {
            Id = ticket.Id,
            Name = ticket.TicketType.ToString(), 
            Price = ticket.Price
        }).ToList();

        return response;
    }
}
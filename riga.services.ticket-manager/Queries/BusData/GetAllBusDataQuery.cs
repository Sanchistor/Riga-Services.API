using MediatR;
using Microsoft.EntityFrameworkCore;
using riga.services.DbContext;
using riga.services.riga.services.ticket_manager.Responses;

namespace riga.services.riga.services.ticket_manager.Queries;

public class GetAllBusDataQuery:IRequest<List<AllBusesResponse>>
{
}

public class GetAllBusDataQueryHandler : IRequestHandler<GetAllBusDataQuery, List<AllBusesResponse>>
{
    private readonly ApiDbContext _context;

    public GetAllBusDataQueryHandler(ApiDbContext context)
    {
        _context = context;
    }
    public async Task<List<AllBusesResponse>> Handle(GetAllBusDataQuery request, CancellationToken cancellationToken)
    {
        return await this.GetAllBusDataAsync(cancellationToken);
    }

    private async Task<List<AllBusesResponse>> GetAllBusDataAsync(CancellationToken cancellationToken)
    {
        var buses = await _context.BusData.ToListAsync(cancellationToken);

        var response = buses.Select(bus => new AllBusesResponse
        {
            Id = bus.Id,
            Type = bus.Type.ToString(), 
            BusNumber = bus.BusNumber,
            BusCode = bus.BusCode
            
        }).ToList();

        return response;
    }
}
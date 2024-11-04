using MediatR;
using riga.services.DbContext;
using riga.services.Models;
using riga.services.riga.services.ticket_manager.DTO;

namespace riga.services.riga.services.ticket_manager.Commands;

public class CreateBusDataCommand:IRequest<bool>
{
    public CreateBusDataDto CreateBusDataDto { get; }

    public CreateBusDataCommand(CreateBusDataDto createBusDataDto)
    {
        CreateBusDataDto = createBusDataDto;
    }
}

public class CreateBusDataCommandHandler : IRequestHandler<CreateBusDataCommand, bool>
{
    private readonly ApiDbContext _context;

    public CreateBusDataCommandHandler(ApiDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Handle(CreateBusDataCommand request, CancellationToken cancellationToken)
    {
        return await this.saveBusInfo(request.CreateBusDataDto);
    }

    private async Task<bool> saveBusInfo(CreateBusDataDto requestCreateBusDataDto)
    {
        try
        {
            if (requestCreateBusDataDto.BusCode < 100000 || requestCreateBusDataDto.BusCode > 999999)
            {
                return false;
            }

            var bus = new BusData
            {
                BusCode = requestCreateBusDataDto.BusCode,
                BusNumber = requestCreateBusDataDto.BusNumber,
                Type = requestCreateBusDataDto.Type
            };
            _context.BusData.Add(bus);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
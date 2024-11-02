using MediatR;
using riga.services.DbContext;
using riga.services.Models;
using riga.services.riga.services.ticket_manager.DTO;
using riga.services.riga.services.ticket_manager.IRepositories;

namespace riga.services.riga.services.ticket_manager.Commands;

public class CreateTicketInfoCommand: IRequest<bool>
{
    public CreateTicketDto CreateTicketDto { get; }

    public CreateTicketInfoCommand(CreateTicketDto createTicketDto)
    {
        CreateTicketDto = createTicketDto;
    }
}

public class CreateTicketInfoCommandHandler : IRequestHandler<CreateTicketInfoCommand, bool>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ApiDbContext _context;

    public CreateTicketInfoCommandHandler(ITicketRepository ticketRepository, ApiDbContext context)
    {
        _ticketRepository = ticketRepository;
        _context = context;
    }
    
    public async Task<bool> Handle(CreateTicketInfoCommand request, CancellationToken cancellationToken)
    {
        return await this.saveTicketInfo(request.CreateTicketDto);
    }

    private async Task<bool> saveTicketInfo(CreateTicketDto createTicketDto)
    {
        try
        {
            var Ticket = new TicketsInfo
            {
                TicketType = createTicketDto.TicketType,
                Price = createTicketDto.Price
            };
            _context.TicketsInfo.Add(Ticket);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
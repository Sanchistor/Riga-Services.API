using MediatR;
using Microsoft.EntityFrameworkCore;
using riga.services.DbContext;
using riga.services.Models;
using riga.services.riga.services.authentication.Services.Guard;
using riga.services.riga.services.ticket_manager.DTO;
using riga.services.riga.services.ticket_manager.IRepositories;
using riga.services.riga.services.ticket_manager.Responses;

namespace riga.services.riga.services.ticket_manager.Commands;

public class CreateTicketInfoCommand: IRequest<AdminResponse>
{
    public CreateTicketDto CreateTicketDto { get; }

    public CreateTicketInfoCommand(CreateTicketDto createTicketDto)
    {
        CreateTicketDto = createTicketDto;
    }
}

public class CreateTicketInfoCommandHandler : IRequestHandler<CreateTicketInfoCommand, AdminResponse>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ApiDbContext _context;
    private readonly AuthGuard _authGuard;

    public CreateTicketInfoCommandHandler(ITicketRepository ticketRepository, ApiDbContext context, AuthGuard authGuard)
    {
        _authGuard = authGuard;
        _ticketRepository = ticketRepository;
        _context = context;
    }
    
    public async Task<AdminResponse> Handle(CreateTicketInfoCommand request, CancellationToken cancellationToken)
    {
        return await this.saveTicketInfo(request.CreateTicketDto, cancellationToken);
    }

    private async Task<AdminResponse> saveTicketInfo(CreateTicketDto createTicketDto, CancellationToken cancellationToken)
    {
        var userGuid = _authGuard.GetUserId();
        var userRole = await _context.Users
            .Where(user => user.Id == userGuid)
            .Select(user => user.Role)
            .FirstOrDefaultAsync(cancellationToken);

        if (userRole == 1)
        {
            try
            {
                var Ticket = new TicketsInfo
                {
                    TicketType = createTicketDto.TicketType,
                    Price = createTicketDto.Price
                };
                _context.TicketsInfo.Add(Ticket);
                await _context.SaveChangesAsync(cancellationToken);
                return new AdminResponse
                {
                    Message = "Modifications saved!",
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new AdminResponse
                {
                    Message = "Error occured!",
                    Success = false
                };
            }
        }
        return new AdminResponse
        {
            Message = "Insufficient privileges, you have to be admin!",
            Success = false
        };
        
    }
}
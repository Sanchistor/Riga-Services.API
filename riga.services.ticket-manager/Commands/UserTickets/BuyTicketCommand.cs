using System.Security.Claims;
using MediatR;
using riga.services.DbContext;
using riga.services.Models;
using riga.services.riga.services.authentication.Services.Guard;
using riga.services.riga.services.ticket_manager.DTO.UserTickets;
using riga.services.riga.services.ticket_manager.IRepositories;

namespace riga.services.riga.services.ticket_manager.Commands.UserTickets;

public class BuyTicketCommand: IRequest<bool>
{
    public BuyTicketDto buyTicketDto { get; set; }

    public BuyTicketCommand(BuyTicketDto _buyTicketDto)
    {
        buyTicketDto = _buyTicketDto;
    }
}

public class BuyTicketCommandHandler : IRequestHandler<BuyTicketCommand, bool>
{
    private readonly AuthGuard _authGuard;
    private readonly ITicketRepository _ticketRepository;

    public BuyTicketCommandHandler(ITicketRepository ticketRepository, AuthGuard authGuard)
    {
        _authGuard = authGuard;
        _ticketRepository = ticketRepository;
    }

    public async Task<bool> Handle(BuyTicketCommand request, CancellationToken cancellationToken)
    {
        return await PurchaseTicketAsync(request.buyTicketDto, cancellationToken);
    }

    private async Task<bool> PurchaseTicketAsync(BuyTicketDto buyTicketDto, CancellationToken cancellationToken)
    {
        //TODO: Make a balance decreasing for user when buying tickets
        var userGuid = _authGuard.GetUserId();
        if (userGuid == null)
        {
            return false;
        }
       

        var ticket = await _ticketRepository.get_ticket_data(buyTicketDto.TicketId, cancellationToken);
        if (ticket == null)
        {
            return false;
        }

        for (int i = 0; i < buyTicketDto.Number; i++)
        {
            var userTicket = new Models.UserTickets
            {
                Id = Guid.NewGuid(),
                Number = 1,
                StartDate = null,
                DueTime = null,
                TicketsId = buyTicketDto.TicketId,
                BusDataId = null,
                UserId = (Guid)userGuid,
                Valid = true
            };
            var saveResult = await _ticketRepository.save_user_tickets(userTicket, cancellationToken);
            if (!saveResult)
            {
                return false; 
            }
        }

        return true;
    }
}

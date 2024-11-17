using System.Security.Claims;
using MediatR;
using riga.services.DbContext;
using riga.services.Models;
using riga.services.riga.services.authentication.Services.Guard;
using riga.services.riga.services.ticket_manager.DTO.UserTickets;
using riga.services.riga.services.ticket_manager.IRepositories;
using riga.services.riga.services.ticket_manager.Responses.UserTickets;

namespace riga.services.riga.services.ticket_manager.Commands.UserTickets;

public class BuyTicketCommand: IRequest<PurchasedTicketResponse>
{
    public BuyTicketDto buyTicketDto { get; set; }

    public BuyTicketCommand(BuyTicketDto _buyTicketDto)
    {
        buyTicketDto = _buyTicketDto;
    }
}

public class BuyTicketCommandHandler : IRequestHandler<BuyTicketCommand, PurchasedTicketResponse>
{
    private readonly AuthGuard _authGuard;
    private readonly ITicketRepository _ticketRepository;

    public BuyTicketCommandHandler(ITicketRepository ticketRepository, AuthGuard authGuard)
    {
        _authGuard = authGuard;
        _ticketRepository = ticketRepository;
    }

    public async Task<PurchasedTicketResponse> Handle(BuyTicketCommand request, CancellationToken cancellationToken)
    {
        return await PurchaseTicketAsync(request.buyTicketDto, cancellationToken);
    }

    private async Task<PurchasedTicketResponse> PurchaseTicketAsync(BuyTicketDto buyTicketDto, CancellationToken cancellationToken)
    {
        var userGuid = _authGuard.GetUserId();

        var ticket = await _ticketRepository.get_ticket_data(buyTicketDto.TicketId, cancellationToken);
        if (ticket == null)
        {
            return new PurchasedTicketResponse
            {
                Message = "Ticket not found",
                Success = false
            };
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
            var ticket_payed = await _ticketRepository.pay_ticket(ticket.Price, (Guid)userGuid, cancellationToken);
            if (!ticket_payed)
            {
                return new PurchasedTicketResponse
                {
                    Message = "Insufficient balance",
                    Success = false
                };
            }
            
            var saveResult = await _ticketRepository.save_user_tickets(userTicket, cancellationToken);
            if (!saveResult)
            {
                return new PurchasedTicketResponse
                {
                    Message = "Error occured",
                    Success = false
                };
            }
        }

        return new PurchasedTicketResponse
        {
            Message = "Ticket purchased",
            Success = true
        };
    }
}

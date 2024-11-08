using MediatR;
using riga.services.Models;
using riga.services.riga.services.authentication.Services.Guard;
using riga.services.riga.services.ticket_manager.DTO.UserTickets;
using riga.services.riga.services.ticket_manager.IRepositories;
using riga.services.riga.services.ticket_manager.Responses.UserTickets;

namespace riga.services.riga.services.ticket_manager.Commands.UserTickets;

public class RegisterTicketCommand: IRequest<RigisteredTicketResponse>
{
    public RegisterTicketDTO _RegisterTicketDto;

    public RegisterTicketCommand(RegisterTicketDTO registerTicketDto)
    {
        _RegisterTicketDto = registerTicketDto;
    }
}

public class RegisterTicketCommandHandler: IRequestHandler<RegisterTicketCommand, RigisteredTicketResponse>
{
    private readonly AuthGuard _authGuard;
    private readonly ITicketRepository _ticketRepository;

    public RegisterTicketCommandHandler(ITicketRepository ticketRepository, AuthGuard authGuard)
    {
        _authGuard = authGuard;
        _ticketRepository = ticketRepository;
    }
    
    public async Task<RigisteredTicketResponse> Handle(RegisterTicketCommand request, CancellationToken cancellationToken)
    {
        return await registerTicketAsync(request._RegisterTicketDto, cancellationToken);
    }

    private async Task<RigisteredTicketResponse> registerTicketAsync(RegisterTicketDTO registerTicketDto, CancellationToken cancellationToken)
    {
        var userGuid = _authGuard.GetUserId();
        var ticket = await _ticketRepository.get_user_unregistered_ticket((Guid)userGuid, registerTicketDto.UserTicketId, cancellationToken);
        if (ticket == null)
        {
            return new RigisteredTicketResponse
            {
                Message = "Ticket not found or already registered",
                Success = false
            };
        }

        var startDate = DateTime.UtcNow;
        DateTime dueTime = startDate;
        switch (ticket.TicketType)
        {
            case TicketType.Min90:
                dueTime = startDate.AddMinutes(90);
                break;
            case TicketType.Hour24:
                dueTime = startDate.AddHours(24);
                break;
            case TicketType.Day3:
                dueTime = startDate.AddDays(3);
                break;
            case TicketType.Day5:
                dueTime = startDate.AddDays(5);
                break;
            case TicketType.Month1:
                dueTime = startDate.AddMonths(1);
                break;
        }

        var busData = await _ticketRepository.get_bus_data_by_code(registerTicketDto.BusCode, cancellationToken);
        if (busData == null)
        {
            return new RigisteredTicketResponse
            {
                Message = "Bus code not found",
                Success = false
            };
        }
        var updateUserTicketsDto = new UpdateUserTicketsDto
        {
            UserTicketId = ticket.UserTicket.Id,
            StartDate = startDate,
            DueTime = dueTime,
            busData = busData
        };
        
        var savedTicket = await _ticketRepository.update_user_ticket(updateUserTicketsDto, cancellationToken);
        if (savedTicket)
        {
            return new RigisteredTicketResponse()
            {
                UserId = (Guid)userGuid,
                BusCode = busData.BusCode,
                CurrentTime = startDate,
                TicketType = ticket.TicketType.ToString(),
                ValidUntil = ticket.UserTicket.DueTime,
                TicketValid = ticket.UserTicket.Valid,
                Success = true,
                Message = "Ticket registered successfully"
            };
        }

        return new RigisteredTicketResponse
        {
            Success = false,
            Message = "Error occured"
        };
    }
    
}
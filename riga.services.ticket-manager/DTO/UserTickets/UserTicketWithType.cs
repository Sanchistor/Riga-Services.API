using riga.services.Models;

namespace riga.services.riga.services.ticket_manager.DTO.UserTickets;

public class UserTicketWithType
{
    public Models.UserTickets UserTicket { get; set; }
    public TicketType TicketType { get; set; }
}
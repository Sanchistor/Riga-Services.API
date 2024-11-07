using riga.services.Models;

namespace riga.services.riga.services.ticket_manager.DTO.UserTickets;

public class UpdateUserTicketsDto
{
    public Guid UserTicketId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueTime { get; set; }
    public BusData busData { get; set; }
}
namespace riga.services.riga.services.ticket_manager.Responses.UserTickets;

public class UnregisteredTicketsResponse
{
    public Guid UserTicketId { get; set; }
    public Guid TicketId { get; set; }
    public string Name { get; set; }
}
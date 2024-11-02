namespace riga.services.riga.services.ticket_manager.DTO;

public class CreateTicketDto
{
    public Models.TicketType TicketType { get; set; }
    public double Price { get; set; }
}
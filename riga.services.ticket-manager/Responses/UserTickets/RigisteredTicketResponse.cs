namespace riga.services.riga.services.ticket_manager.Responses.UserTickets;

public class RigisteredTicketResponse
{
    public Guid UserId { get; set; }
    public int BusCode { get; set; }
    public string TicketType { get; set; }
    public bool TicketValid { get; set; }
    public DateTime? ValidUntil { get; set; }
    public DateTime CurrentTime { get; set; }
    public bool? Success { get; set; }      
    public string? Message { get; set; } 
}
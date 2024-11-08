namespace riga.services.riga.services.ticket_manager.Responses.UserTickets;

public class HistoryOfTripsResponse
{
    public int BusCode { get; set; }
    public string TicketType { get; set; }
    public DateTime DateOfIssue { get; set; }
}
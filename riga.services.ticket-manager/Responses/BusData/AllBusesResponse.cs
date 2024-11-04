namespace riga.services.riga.services.ticket_manager.Responses;

public class AllBusesResponse
{
    public Guid Id { get; set; }
    public int BusNumber { get; set; }
    public int BusCode { get; set; }
    public string Type { get; set; }
}
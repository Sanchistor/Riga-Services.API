using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace riga.services.Models;

public enum Type
{
    Bus,
    TrolleyBus,
    Tram,
    BusTaxi
}

public class BusData
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public int BusNumber { get; set; }
    public int BusCode { get; set; }
    public Type Type { get; set; }
    [JsonIgnore]
    public ICollection<UserTickets> UserTickets { get; set; } = new List<UserTickets>();
}
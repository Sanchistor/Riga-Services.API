using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace riga.services.Models;

public enum TicketType
{
    Min90,
    Hour24,
    Day3,
    Day5,
    Month1
}

public class TicketsInfo
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public TicketType TicketType { get; set; }
    public double Price { get; set; }
    [JsonIgnore]
    public ICollection<UserTickets> UserTickets { get; set; } = new List<UserTickets>();
}
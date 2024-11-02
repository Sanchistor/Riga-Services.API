using System.ComponentModel.DataAnnotations.Schema;

namespace riga.services.Models;

public class UserTickets
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public int Number { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueTime { get; set; }
    
    // Foreign Key for the 1:N relationship with TicketsInfo table
    public Guid TicketsId { get; set; }
    public TicketsInfo Tickets { get; set; }
    
    // Foreign Key for the 1:N relationship with BusData table
    public Guid BusDataId { get; set; }
    public BusData BusData { get; set; }
    
    // Foreign Key for the 1:N relationship with Users table
    public Guid UserId { get; set; }
    public User User { get; set; }
}
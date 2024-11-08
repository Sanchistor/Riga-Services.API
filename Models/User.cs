using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace riga.services.Models;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    [JsonIgnore]
    public string Password { get; set; }
    [JsonIgnore]
    
    public int Role { get; set; } = 0;
    public double Balance { get; set; } = 0.0;
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    
    [JsonIgnore]
    public ICollection<UserTickets> UserTickets { get; set; } = new List<UserTickets>();
}
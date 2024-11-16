using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace riga.services.Models;

public class CreditCard
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public int CardNum { get; set; }
    public String Date { get; set; }
    public int Cvv { get; set; }

    
    public Guid UserId { get; set; }
    public User User { get; set; }
}
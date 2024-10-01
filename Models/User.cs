using System.ComponentModel.DataAnnotations.Schema;

namespace riga.services.Models;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public int Role { get; set; } = 0;
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}
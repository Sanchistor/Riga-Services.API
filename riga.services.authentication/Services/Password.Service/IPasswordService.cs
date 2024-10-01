namespace riga.services.riga.services.authentication.Services;

public interface IPasswordService
{
    public string HashPassword(string password);
    public bool VerifyPassword(string password, string hashedPassword);   
}
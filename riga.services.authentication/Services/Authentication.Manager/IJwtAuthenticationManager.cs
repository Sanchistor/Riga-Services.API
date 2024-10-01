namespace riga.services.riga.services.authentication.Services;

public interface IJwtAuthenticationManager
{
    string Authenticate(string email, string password);
}
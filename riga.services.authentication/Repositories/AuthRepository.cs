using MediatR;
using Microsoft.EntityFrameworkCore;
using riga.services.DbContext;
using riga.services.Models;
using riga.services.riga.services.authentication.DTO;
using riga.services.riga.services.authentication.IRepositories;
using riga.services.riga.services.authentication.Services;

namespace riga.services.riga.services.authentication.Repositories;

public class AuthRepository: IAuthRepository
{
    private readonly ApiDbContext _context;
    private readonly IPasswordService _passwordService;
    public AuthRepository(ApiDbContext context, IPasswordService passwordService)
    {
        _context = context;
        _passwordService = passwordService;
    }
    public async Task<Unit> CreateUserRecord(CreateProfileDto createProfileDto)
    {
        var user = new User
        {
            FirstName = createProfileDto.FirstName,
            LastName = createProfileDto.LastName,
            Email = createProfileDto.Email,
            Phone = createProfileDto.Phone,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow
        };

        // Hash the user's password before saving it
        string hashedPassword = _passwordService.HashPassword(createProfileDto.Password);
        user.Password = hashedPassword;
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Unit.Value;
    }

    public async Task<User> GetProfile(Guid UserId, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Id == UserId, cancellationToken);
    }
}
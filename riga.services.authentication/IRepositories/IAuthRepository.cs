using MediatR;
using riga.services.Models;
using riga.services.riga.services.authentication.DTO;

namespace riga.services.riga.services.authentication.IRepositories;

public interface IAuthRepository
{
    public Task<Unit> CreateUserRecord(CreateProfileDto createProfileDto);
    public Task<User> GetProfile(Guid UserId, CancellationToken cancellationToken);
}
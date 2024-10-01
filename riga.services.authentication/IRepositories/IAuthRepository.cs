using MediatR;
using riga.services.riga.services.authentication.DTO;

namespace riga.services.riga.services.authentication.IRepositories;

public interface IAuthRepository
{
    public Task<Unit> CreateUserRecord(CreateProfileDto createProfileDto);
}
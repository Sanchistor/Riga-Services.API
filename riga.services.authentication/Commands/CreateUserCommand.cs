using MediatR;
using riga.services.riga.services.authentication.DTO;
using riga.services.riga.services.authentication.IRepositories;

namespace riga.services.riga.services.authentication.Commands;


public class CreateUserCommand:IRequest {
    public CreateProfileDto createProfileDto { get; set; }
}
public class CreateUserHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IAuthRepository _authRepository;
    public CreateUserHandler(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }
    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await _authRepository.CreateUserRecord(request.createProfileDto);
    }
}
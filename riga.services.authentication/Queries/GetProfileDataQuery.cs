using MediatR;
using riga.services.Models;
using riga.services.riga.services.authentication.IRepositories;
using riga.services.riga.services.authentication.Services.Guard;

namespace riga.services.riga.services.authentication.Queries;

public class GetProfileDataQuery: IRequest<User>
{
    
}

public class GetProfileDataQueryHandler: IRequestHandler<GetProfileDataQuery, User>
{
    private readonly AuthGuard _authGuard;
    private readonly IAuthRepository _authRepository;

    public GetProfileDataQueryHandler(IAuthRepository authRepository, AuthGuard authGuard)
    {
        _authGuard = authGuard;
        _authRepository = authRepository;
    }
    public async Task<User> Handle(GetProfileDataQuery request, CancellationToken cancellationToken)
    {
        return await this.GetProfileAsync(cancellationToken);
    }

    private async Task<User> GetProfileAsync(CancellationToken cancellationToken)
    {
        var userGuid = (Guid)_authGuard.GetUserId();
        return await _authRepository.GetProfile(userGuid, cancellationToken);
    }
}
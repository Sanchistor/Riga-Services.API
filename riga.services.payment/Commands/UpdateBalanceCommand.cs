using MediatR;
using riga.services.Models;
using riga.services.riga.services.authentication.Services.Guard;
using riga.services.riga.services.payment.DTO;
using riga.services.riga.services.payment.IRepositories;
using riga.services.riga.services.payment.Responses;

namespace riga.services.riga.services.payment.Commands;

public class UpdateBalanceCommand : IRequest<BalanceUpdatedResponse>
{
    public BalanceDto BalanceDto { get; set; }

    public UpdateBalanceCommand(BalanceDto balanceDto)
    {
        BalanceDto = balanceDto;
    }
}

public class UpdateBalanceCommandHandler : IRequestHandler<UpdateBalanceCommand, BalanceUpdatedResponse>
{
    
    private readonly ICardDataRepository _cardDataRepository;
    private readonly AuthGuard _authGuard;
    
    public UpdateBalanceCommandHandler(ICardDataRepository cardDataRepository, AuthGuard authGuard)
    {
        _cardDataRepository = cardDataRepository;
        _authGuard = authGuard;
    }
    
    // public async Task<BalanceUpdatedResponse> Handle(UpdateBalanceCommand request, CancellationToken cancellationToken)
    // {
    //     // throw new NotImplementedException();
    //     return await this.UpdateBalance(request.BalanceDto, cancellationToken);
    // }
    //
    // private async Task<BalanceUpdatedResponse> UpdateBalance(BalanceDto balanceDto, CreditCard creditCard, CancellationToken cancellationToken)
    // {
    //     
    //
    //     return null;
    // }

    public Task<BalanceUpdatedResponse> Handle(UpdateBalanceCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

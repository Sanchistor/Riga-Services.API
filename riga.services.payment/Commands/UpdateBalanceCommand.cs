using System.Windows.Input;
using MediatR;
using riga.services.riga.services.authentication.Services.Guard;
using riga.services.riga.services.payment.DTO;
using riga.services.riga.services.payment.IRepositories;
using riga.services.riga.services.payment.Responses;

namespace riga.services.riga.services.payment.Commands;

public class UpdateBalanceCommand : IRequest<BalanceUpdatedResponse> {
    public BalanceDto balanceDto { get; set; }
    
    public UpdateBalanceCommand(BalanceDto _balanceDto) {
        balanceDto = _balanceDto;
    }

}

public class UpdateBalanceCommandHandler : IRequestHandler<UpdateBalanceCommand, BalanceUpdatedResponse> {
    
    private readonly AuthGuard _authGuard;
    private readonly IBalanceRepository _balanceRepository;

    public UpdateBalanceCommandHandler(IBalanceRepository balanceRepository, AuthGuard authGuard)
    {
        _authGuard = authGuard;
        _balanceRepository = balanceRepository;
    }

    
    public async Task<BalanceUpdatedResponse> Handle(UpdateBalanceCommand request, CancellationToken cancellationToken) {
        
        return await this.UpdateBalance(request.balanceDto, cancellationToken);
    }

    private async Task<BalanceUpdatedResponse> UpdateBalance(BalanceDto _balanceDto, CancellationToken cancellationToken) {

        
        return null;
    }
    
}
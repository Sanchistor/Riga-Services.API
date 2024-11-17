using MediatR;
using riga.services.Models;
using riga.services.riga.services.authentication.Services.Guard;
using riga.services.riga.services.payment.DTO;
using riga.services.riga.services.payment.IRepositories;
using riga.services.riga.services.payment.Responses;

namespace riga.services.riga.services.payment.Commands;

public class UpdateBalanceCommand : IRequest<BalanceUpdatedResponse>
{
    public CardDataDto CardDataDto { get; }

    public UpdateBalanceCommand(CardDataDto cardDataDto)
    {
        CardDataDto = cardDataDto;
    }
}

public class UpdateBalanceCommandHandler : IRequestHandler<UpdateBalanceCommand, BalanceUpdatedResponse>
{
    private readonly AuthGuard _authGuard;
    private readonly IUpdateBalanceRepository _repository;
    private readonly ICardDataRepository _cardDataRepository;

    public UpdateBalanceCommandHandler(IUpdateBalanceRepository balanceRepository, ICardDataRepository cardDataRepository, AuthGuard authGuard)
    {
        _authGuard = authGuard;
        _repository = balanceRepository;
        _cardDataRepository = cardDataRepository;
    }

    public async Task<BalanceUpdatedResponse> Handle(UpdateBalanceCommand request, CancellationToken cancellationToken)
    {
        
        var userId = _authGuard.GetUserId();
        if (_cardDataRepository.GetCreditCard((Guid)userId, request.CardDataDto.CardNum, cancellationToken)==null)
        {
            return new BalanceUpdatedResponse
            {
                Succes =  false,
                Message = "Balance not updated. Card not matches to user."
            };
        }
        var success = await _repository.IncreaseBalance(request.CardDataDto, cancellationToken);
        if (!success)
        {
            return new BalanceUpdatedResponse
            {
                Succes =  false,
                Message = "Balance not updated. Errors occured"
            };
        }

        return new BalanceUpdatedResponse
        {
            Succes =  true,
            Message = "Balance updated successfully"
        };
    }
}


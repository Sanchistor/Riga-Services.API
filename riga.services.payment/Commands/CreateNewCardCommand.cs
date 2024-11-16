using MediatR;
using riga.services.riga.services.authentication.Services.Guard;
using riga.services.riga.services.payment.DTO;
using riga.services.riga.services.payment.IRepositories;
using riga.services.riga.services.payment.Responses;

namespace riga.services.riga.services.payment.Commands;

public class CreateNewCardCommand : IRequest<CreateCardResponse>
{
    public CardDataDto CardDataDto { get; set; }

    public CreateNewCardCommand(CardDataDto cardDataDto)
    {
        CardDataDto = cardDataDto;
    }
}

public class CreateNewCardCommandHandler : IRequestHandler<CreateNewCardCommand, CreateCardResponse>
{
    private readonly ICardDataRepository _cardDataRepository;
    private readonly AuthGuard _authGuard;
    
    public CreateNewCardCommandHandler(ICardDataRepository cardDataRepository, AuthGuard authGuard)
    {
        _cardDataRepository = cardDataRepository;
        _authGuard = authGuard;
    }

    public async Task<CreateCardResponse> Handle(CreateNewCardCommand request, CancellationToken cancellationToken)
    {
        var userGuid = _authGuard.GetUserId();
        // var response = await _balanceRepository.AddBalance(request.BalanceDto, (Guid)userGuid, cancellationToken);
        var userCard = await _cardDataRepository.GetCreditCard((Guid)userGuid, request.CardDataDto.CardNum, cancellationToken);

        if (userCard != null)
        {
            return new CreateCardResponse
            {
                Message = "Current card already exists! Try another one",
                Success = false
            };
        }
        
        var result = await _cardDataRepository.CreateNewCreditCard((Guid)userGuid, request.CardDataDto, cancellationToken);

        if (result)
        {
            return new CreateCardResponse
            {
                Message = "Card successfully added!",
                Success = true
            };        
        }
        return new CreateCardResponse
        {
            Message = "Something went wrong! Try again!",
            Success = false
        };

    }
}


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
    private readonly IUpdateBalanceRepository _repository;

    public UpdateBalanceCommandHandler(IUpdateBalanceRepository repository)
    {
        _repository = repository;
    }

    public async Task<BalanceUpdatedResponse> Handle(UpdateBalanceCommand request, CancellationToken cancellationToken)
    {
        var success = await _repository.IncreaseBalance(request.CardDataDto, cancellationToken);
        if (!success)
        {
            return null;
        }

        return new BalanceUpdatedResponse
        {
            Succes =  true,
            Message = "Balance updated successfully"
        };
    }
}


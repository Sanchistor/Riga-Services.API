using riga.services.Models;
using riga.services.riga.services.payment.DTO;
using riga.services.riga.services.payment.Responses;

namespace riga.services.riga.services.payment.IRepositories;

public interface ICardDataRepository
{

    public Task<BalanceUpdatedResponse> AddBalance(CardDataDto cardDataDto, Guid userGuid, CancellationToken cancellationToken);

    public Task<CreditCard> GetCreditCard(Guid userGuid, String CardNum, CancellationToken cancellationToken);

    public Task<bool> CreateNewCreditCard(Guid userGuid, CardDataDto cardDataDto, CancellationToken cancellationToken);
    
    // public Task<bool> updateBalance(CreditCard creditCard, double , CancellationToken cancellationToken);
}
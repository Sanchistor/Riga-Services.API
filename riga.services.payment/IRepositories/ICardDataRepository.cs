using riga.services.Models;
using riga.services.riga.services.payment.DTO;
using riga.services.riga.services.payment.Responses;

namespace riga.services.riga.services.payment.IRepositories;

public interface ICardDataRepository
{
    public Task<CreditCard> GetCreditCard(Guid userGuid, String cardNum, CancellationToken cancellationToken);
    public Task<Guid?> GetUserIdByCardNumber(string cardNum, CancellationToken cancellationToken);
    public Task<bool> CreateNewCreditCard(Guid userGuid, CardDataDto cardDataDto, CancellationToken cancellationToken);
}
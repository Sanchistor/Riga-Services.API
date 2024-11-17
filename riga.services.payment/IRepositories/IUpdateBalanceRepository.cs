using riga.services.Models;
using riga.services.riga.services.payment.DTO;

namespace riga.services.riga.services.payment.IRepositories;

public interface IUpdateBalanceRepository
{
    public Task<bool> IncreaseBalance(CardDataDto cardDataDto, Guid userId, CancellationToken cancellationToken);
}
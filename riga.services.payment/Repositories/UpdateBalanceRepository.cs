using Microsoft.EntityFrameworkCore;
using riga.services.DbContext;

using riga.services.riga.services.payment.DTO;
using riga.services.riga.services.payment.IRepositories;

namespace riga.services.riga.services.payment.Repositories;

public class UpdateBalanceRepository:IUpdateBalanceRepository
{
    
    private readonly ApiDbContext _context;
    
    public UpdateBalanceRepository(ApiDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> IncreaseBalance(CardDataDto cardDataDto, CancellationToken cancellationToken)
    {

        var existingCard = await _context.CreditCard
            .FirstOrDefaultAsync(c => c.CardNum == cardDataDto.CardNum, cancellationToken);

        if (existingCard == null)
        {
            return false;
        }

        existingCard.Balance += cardDataDto.balance;

        _context.CreditCard.Update(existingCard);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

}
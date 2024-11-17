using Microsoft.EntityFrameworkCore;
using riga.services.DbContext;
using riga.services.Models;
using riga.services.riga.services.payment.DTO;
using riga.services.riga.services.payment.IRepositories;

namespace riga.services.riga.services.payment.Repositories;

public class UpdateBalanceRepository : IUpdateBalanceRepository
{
    private readonly ApiDbContext _context;


    public UpdateBalanceRepository(ApiDbContext context, ICardDataRepository cardDataRepository)
    {
        _context = context;

    }

    public async Task<bool> IncreaseBalance(CardDataDto cardDataDto, Guid userId, CancellationToken cancellationToken)
    {

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    
        if (user == null)
        {
            return false;
        }

        user.Balance += cardDataDto.Balance;

        _context.Users.Update(user);
    
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

}

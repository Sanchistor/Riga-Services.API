using Microsoft.EntityFrameworkCore;
using riga.services.DbContext;
using riga.services.Models;
using riga.services.riga.services.payment.DTO;
using riga.services.riga.services.payment.IRepositories;
using riga.services.riga.services.payment.Responses;

namespace riga.services.riga.services.payment.Repositories;
public class CardDataRepository : ICardDataRepository
{
    private readonly ApiDbContext _context;
    
    public CardDataRepository(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<BalanceUpdatedResponse> AddBalance(CardDataDto cardDataDto, Guid userGuid, CancellationToken cancellationToken)
    {
        //
        // var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == creditCard.UserId);
        //
        // if (user == null)
        // {
        //     return new BalanceUpdatedResponse
        //     {
        //         Success = false,
        //         Message = "User not found."
        //     };
        // }
        //
        // // Add the balance to the user's account
        // user.Balance += balanceDto.Amount;
        // _context.Users.Update(user);
        // await _context.SaveChangesAsync();
        //
        // return new BalanceUpdatedResponse
        // {
        //     Success = true,
        //     Message = "Balance updated successfully."
        // };
        return null;
    }

    public async Task<CreditCard> GetCreditCard(Guid userGuid, String CardNum, CancellationToken cancellationToken)
    {
        var creditCard = await _context.CreditCard
            .FirstOrDefaultAsync(c => c.UserId == userGuid && c.CardNum == CardNum, cancellationToken);

      return creditCard;
    }

    public async Task<bool> CreateNewCreditCard(Guid userGuid, CardDataDto cardDataDto, CancellationToken cancellationToken)
    {
        var newCard = new CreditCard
        {
            CardNum = cardDataDto.CardNum,
            Date = cardDataDto.Date,
            Cvv = cardDataDto.Cvv,
            UserId = userGuid
        };
        
        _context.CreditCard.Add(newCard);
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
    
}

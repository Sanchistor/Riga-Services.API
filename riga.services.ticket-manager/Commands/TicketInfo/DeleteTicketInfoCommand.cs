using MediatR;
using Microsoft.EntityFrameworkCore;
using riga.services.DbContext;
using riga.services.riga.services.authentication.Services.Guard;
using riga.services.riga.services.ticket_manager.Responses;

namespace riga.services.riga.services.ticket_manager.Commands;

public class DeleteTicketInfoCommand:IRequest<AdminResponse>
{
    public Guid Id { get; set; }

    public DeleteTicketInfoCommand(Guid id)
    {
        Id = id;
    }
}

public class DeleteTicketInfoCommandHandler: IRequestHandler<DeleteTicketInfoCommand, AdminResponse>
{
    private readonly ApiDbContext _context;
    private readonly AuthGuard _authGuard;

    public DeleteTicketInfoCommandHandler(ApiDbContext context,  AuthGuard authGuard)
    {
        _context = context;
        _authGuard = authGuard;
    }
    
    public async Task<AdminResponse> Handle(DeleteTicketInfoCommand request, CancellationToken cancellationToken)
    {
        return await this.DeleteAsync(request.Id, cancellationToken);
    }

    private async Task<AdminResponse> DeleteAsync(Guid requestId, CancellationToken cancellationToken)
    {
        var userGuid = _authGuard.GetUserId();
        var userRole = await _context.Users
            .Where(user => user.Id == userGuid)
            .Select(user => user.Role)
            .FirstOrDefaultAsync(cancellationToken);

        if (userRole == 1)
        {
            var ticket = await _context.TicketsInfo.FindAsync(new object[] { requestId }, cancellationToken);
            if (ticket == null)
            {
                return new AdminResponse
                {
                    Message = "Ticket data not found!",
                    Success = false
                };
            }

            _context.TicketsInfo.Remove(ticket);
            await _context.SaveChangesAsync(cancellationToken);
            return new AdminResponse
            {
                Message = "Ticket data deleted!",
                Success = true
            };
        }
        return new AdminResponse
        {
            Message = "Insufficient privileges, you have to be admin!",
            Success = true
        };
    }
}
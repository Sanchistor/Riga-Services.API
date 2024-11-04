using MediatR;
using riga.services.DbContext;

namespace riga.services.riga.services.ticket_manager.Commands;

public class DeleteTicketInfoCommand:IRequest<bool>
{
    public Guid Id { get; set; }

    public DeleteTicketInfoCommand(Guid id)
    {
        Id = id;
    }
}

public class DeleteTicketInfoCommandHandler: IRequestHandler<DeleteTicketInfoCommand, bool>
{
    private readonly ApiDbContext _context;

    public DeleteTicketInfoCommandHandler(ApiDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Handle(DeleteTicketInfoCommand request, CancellationToken cancellationToken)
    {
        return await this.DeleteAsync(request.Id, cancellationToken);
    }

    private async Task<bool> DeleteAsync(Guid requestId, CancellationToken cancellationToken)
    {
        var ticket = await _context.TicketsInfo.FindAsync(new object[] { requestId }, cancellationToken);
        if (ticket == null)
        {
            return false;
        }

        _context.TicketsInfo.Remove(ticket);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
using MediatR;
using riga.services.DbContext;

namespace riga.services.riga.services.ticket_manager.Commands;

public class DeleteBusDataCommand:IRequest<bool>
{
    public Guid Id { get; set; }

    public DeleteBusDataCommand(Guid id)
    {
        Id = id;
    }
}

public class DeleteBusDataCommandHandler: IRequestHandler<DeleteBusDataCommand, bool>
{
    private readonly ApiDbContext _context;

    public DeleteBusDataCommandHandler(ApiDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Handle(DeleteBusDataCommand request, CancellationToken cancellationToken)
    {
        return await DeleteAsync(request.Id, cancellationToken);
    }

    private async Task<bool> DeleteAsync(Guid requestId, CancellationToken cancellationToken)
    {
        var bus = await _context.BusData.FindAsync(new object[] { requestId }, cancellationToken);
        if (bus == null)
        {
            return false;
        }

        _context.BusData.Remove(bus);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
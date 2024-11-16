using MediatR;
using Microsoft.EntityFrameworkCore;
using riga.services.DbContext;
using riga.services.riga.services.authentication.Services.Guard;
using riga.services.riga.services.ticket_manager.Responses;

namespace riga.services.riga.services.ticket_manager.Commands;

public class DeleteBusDataCommand:IRequest<AdminResponse>
{
    public Guid Id { get; set; }

    public DeleteBusDataCommand(Guid id)
    {
        Id = id;
    }
}

public class DeleteBusDataCommandHandler: IRequestHandler<DeleteBusDataCommand, AdminResponse>
{
    private readonly ApiDbContext _context;
    private readonly AuthGuard _authGuard;

    public DeleteBusDataCommandHandler(ApiDbContext context, AuthGuard authGuard)
    {
        _authGuard = authGuard;
        _context = context;
    }
    
    public async Task<AdminResponse> Handle(DeleteBusDataCommand request, CancellationToken cancellationToken)
    {
        return await DeleteAsync(request.Id, cancellationToken);
    }

    private async Task<AdminResponse> DeleteAsync(Guid requestId, CancellationToken cancellationToken)
    {
        var userGuid = _authGuard.GetUserId();
        var userRole = await _context.Users
            .Where(user => user.Id == userGuid)
            .Select(user => user.Role)
            .FirstOrDefaultAsync();
        
        if (userRole == 1)
        {
            var bus = await _context.BusData.FindAsync(new object[] { requestId }, cancellationToken);
            if (bus == null)
            {
                return new AdminResponse
                {
                    Message = "Bus data not found!",
                    Success = false
                };
            }

            _context.BusData.Remove(bus);
            await _context.SaveChangesAsync(cancellationToken);

            return new AdminResponse
            {
                Message = "Bus data deleted!",
                Success = true
            };
        }
        return new AdminResponse
        {
            Message = "Insufficient privileges, you have to be admin!",
            Success = false
        };
    }
}
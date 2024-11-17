using MediatR;
using Microsoft.EntityFrameworkCore;
using riga.services.DbContext;
using riga.services.Models;
using riga.services.riga.services.authentication.Services.Guard;
using riga.services.riga.services.ticket_manager.DTO;
using riga.services.riga.services.ticket_manager.Responses;

namespace riga.services.riga.services.ticket_manager.Commands;

public class CreateBusDataCommand:IRequest<AdminResponse>
{
    public CreateBusDataDto CreateBusDataDto { get; }

    public CreateBusDataCommand(CreateBusDataDto createBusDataDto)
    {
        CreateBusDataDto = createBusDataDto;
    }
}

public class CreateBusDataCommandHandler : IRequestHandler<CreateBusDataCommand, AdminResponse>
{
    private readonly AuthGuard _authGuard;
    private readonly ApiDbContext _context;

    public CreateBusDataCommandHandler(ApiDbContext context, AuthGuard authGuard)
    {
        _authGuard = authGuard;
        _context = context;
    }
    
    public async Task<AdminResponse> Handle(CreateBusDataCommand request, CancellationToken cancellationToken)
    {
        return await this.saveBusInfo(request.CreateBusDataDto, cancellationToken);
    }

    private async Task<AdminResponse> saveBusInfo(CreateBusDataDto requestCreateBusDataDto, CancellationToken cancellationToken)
    {
        var userGuid = _authGuard.GetUserId();
        var userRole = await _context.Users
            .Where(user => user.Id == userGuid)
            .Select(user => user.Role)
            .FirstOrDefaultAsync(cancellationToken);
        if (userRole == 1)
        {
            try
            {
                if (requestCreateBusDataDto.BusCode < 100000 || requestCreateBusDataDto.BusCode > 999999)
                {
                    return new AdminResponse
                    {
                        Message = "Insufficient Code!",
                        Success = false
                    };
                }

                var bus = new BusData
                {
                    BusCode = requestCreateBusDataDto.BusCode,
                    BusNumber = requestCreateBusDataDto.BusNumber,
                    Type = requestCreateBusDataDto.Type
                };
                _context.BusData.Add(bus);
                await _context.SaveChangesAsync(cancellationToken);
                return new AdminResponse
                {
                    Message = "Modifications saved!",
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new AdminResponse
                {
                    Message = "Error occured!",
                    Success = false
                };
            }
        }

        return new AdminResponse
        {
            Message = "Insufficient privileges, you have to be admin!",
            Success = false
        };
    }
}
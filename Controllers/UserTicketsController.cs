using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using riga.services.riga.services.ticket_manager.Commands;
using riga.services.riga.services.ticket_manager.Commands.UserTickets;
using riga.services.riga.services.ticket_manager.DTO.UserTickets;
using riga.services.riga.services.ticket_manager.Queries.UserTickets;
using riga.services.riga.services.ticket_manager.Responses.UserTickets;

namespace riga.services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserTicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserTicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost, Route("purchaseTicket")]
        public async Task<PurchasedTicketResponse> Post([FromBody] BuyTicketDto buyTicketDto)
        {
            return await _mediator.Send(new BuyTicketCommand(buyTicketDto));
        }
        
        [HttpPost, Route("registerTicket")]
        public async Task<RigisteredTicketResponse> Post([FromBody] RegisterTicketDTO registerTicketDto)
        {
            return await _mediator.Send(new RegisterTicketCommand(registerTicketDto));
        }
        
        [HttpGet, Route("unregisteredTickets")]
        public async Task<List<UnregisteredTicketsResponse>> Get()
        {
            return await _mediator.Send(new GetUnregisteredTicketsQuery());
        }
        
        [HttpGet, Route("validTickets")]
        public async Task<List<RigisteredTicketResponse>> GetValid()
        {
            return await _mediator.Send(new GetValidTicketsQuery());
        }
        
        [HttpGet, Route("historyOfTrips")]
        public async Task<List<HistoryOfTripsResponse>> GetHistory()
        {
            return await _mediator.Send(new GetHistoryOfTripsQuery());
        }
    }
}

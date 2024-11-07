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
        public async Task<bool> Post([FromBody] BuyTicketDto buyTicketDto)
        {
            return await _mediator.Send(new BuyTicketCommand(buyTicketDto));
        }
        
        [HttpPost, Route("registerTicket")]
        public async Task<RigisteredTicketResponse> Post([FromBody] RegisterTicketDTO registerTicketDto)
        {
            return await _mediator.Send(new RegisterTicketCommand(registerTicketDto));
        }
    }
}

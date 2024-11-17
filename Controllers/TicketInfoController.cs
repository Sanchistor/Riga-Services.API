using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using riga.services.riga.services.ticket_manager.Commands;
using riga.services.riga.services.ticket_manager.DTO;
using riga.services.riga.services.ticket_manager.Queries;
using riga.services.riga.services.ticket_manager.Responses;

namespace riga.services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketInfoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketInfoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        // GET: api/Ticket
        [HttpGet]
        public async Task<ActionResult<List<AllTicketsTypeResponse>>> GetAllTickets()
        {
            var query = new GetAllTicketsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // POST: api/Ticket
        [HttpPost]
        public async Task<AdminResponse> Post([FromBody] CreateTicketDto createTicketDto)
        {
            var command = new CreateTicketInfoCommand(createTicketDto);
            var result = await _mediator.Send(command);
            return result;
        }
        
        [HttpDelete("{id}")]
        public async Task<AdminResponse> Delete(Guid id)
        {
            var command = new DeleteTicketInfoCommand(id);
            return await _mediator.Send(command);
        }
    }
}

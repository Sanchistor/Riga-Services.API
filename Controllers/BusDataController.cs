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
    public class BusDataController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BusDataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/BusData/5
        [HttpGet( Name = "Get")]
        public async Task<ActionResult<List<AllBusesResponse>>> Get()
        {
            var query = new GetAllBusDataQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // POST: api/BusData
        //TODO: add logic that only admin can modify post
        [HttpPost]
        public async Task<bool> Post([FromBody] CreateBusDataDto createBusDataDto)
        {
            var command = new CreateBusDataCommand(createBusDataDto);
            var result = await _mediator.Send(command);
            return result;
        }

        // DELETE: api/BusData/5
        //TODO: add logic that only admin can modify delete
        [HttpDelete("{id}")]
        public async Task<bool> Delete(Guid id)
        {
            var command = new DeleteBusDataCommand(id);
            var result = await _mediator.Send(command);
            return result;
        }
    }
}

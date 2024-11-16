using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using riga.services.riga.services.payment.Commands;
using riga.services.riga.services.payment.DTO;
using riga.services.riga.services.payment.Responses;

namespace riga.services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/Payment/createCard
        [HttpPost("createCard")]
        //TODO: make cvv string to handle 0 on the beginning and handle number, date, cvv validation
        public async Task<ActionResult<BalanceUpdatedResponse>> AddBalance([FromBody] CardDataDto cardDataDto)
        {
            var response = await _mediator.Send(new CreateNewCardCommand(cardDataDto));
            return Ok(response);
        }
        
        [HttpPost("updateBalance")]
        //TODO: Finish this endpoint to handle specific card number
        public async Task<ActionResult<BalanceUpdatedResponse>> UpdateBalance([FromBody] BalanceDto balanceDto)
        {
            var response = await _mediator.Send(new UpdateBalanceCommand(balanceDto));
            return Ok(response);
        }
        //TODO: Make method to get all cards for the user
    }
}

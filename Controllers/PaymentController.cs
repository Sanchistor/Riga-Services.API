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
        public async Task<ActionResult<BalanceUpdatedResponse>> AddBalance([FromBody] CardDataDto cardDataDto)
        {
            var response = await _mediator.Send(new CreateNewCardCommand(cardDataDto));
            return Ok(response);
        }

    }
}

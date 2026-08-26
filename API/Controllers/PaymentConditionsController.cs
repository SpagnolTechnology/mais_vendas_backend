using API.Base;
using AppService.AppService.Interfaces;
using Crosscutting.DTO.PaymentCondition;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PaymentConditionsController : BaseController<PaymentConditionsController>
    {
        private readonly IPaymentConditionAppService _paymentConditionAppService;

        public PaymentConditionsController(
            ILogger<PaymentConditionsController> logger,
            IPaymentConditionAppService paymentConditionAppService) : base(logger)
        {
            _paymentConditionAppService = paymentConditionAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PaymentConditionResponseDTO>>> GetAll(CancellationToken ct)
        {
            IReadOnlyList<PaymentConditionResponseDTO> response = await _paymentConditionAppService.GetAllAsync(ct);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PaymentConditionResponseDTO>> GetById(int id)
        {
            PaymentConditionResponseDTO response = await _paymentConditionAppService.GetResponseByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentConditionResponseDTO>> Create([FromBody] CreatePaymentConditionRequestDTO request, CancellationToken ct)
        {
            PaymentConditionResponseDTO response = await _paymentConditionAppService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<PaymentConditionResponseDTO>> Update(int id, [FromBody] UpdatePaymentConditionRequestDTO request)
        {
            PaymentConditionResponseDTO response = await _paymentConditionAppService.UpdateAsync(id, request);
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _paymentConditionAppService.DeleteAsync(id);
            return NoContent();
        }
    }
}

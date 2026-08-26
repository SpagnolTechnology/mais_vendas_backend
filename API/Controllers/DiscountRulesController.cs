using API.Base;
using AppService.AppService.Interfaces;
using Crosscutting.DTO.DiscountRule;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DiscountRulesController : BaseController<DiscountRulesController>
    {
        private readonly IDiscountRuleAppService _discountRuleAppService;

        public DiscountRulesController(
            ILogger<DiscountRulesController> logger,
            IDiscountRuleAppService discountRuleAppService) : base(logger)
        {
            _discountRuleAppService = discountRuleAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<DiscountRuleResponseDTO>>> GetAll(CancellationToken ct)
        {
            IReadOnlyList<DiscountRuleResponseDTO> response = await _discountRuleAppService.GetAllAsync(ct);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DiscountRuleResponseDTO>> GetById(int id)
        {
            DiscountRuleResponseDTO response = await _discountRuleAppService.GetResponseByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<DiscountRuleResponseDTO>> Create([FromBody] CreateDiscountRuleRequestDTO request, CancellationToken ct)
        {
            DiscountRuleResponseDTO response = await _discountRuleAppService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<DiscountRuleResponseDTO>> Update(int id, [FromBody] UpdateDiscountRuleRequestDTO request)
        {
            DiscountRuleResponseDTO response = await _discountRuleAppService.UpdateAsync(id, request);
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _discountRuleAppService.DeleteAsync(id);
            return NoContent();
        }
    }
}

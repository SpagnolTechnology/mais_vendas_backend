using API.Base;
using AppService.AppService.Interfaces;
using Crosscutting.DTO.CommissionRule;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CommissionRulesController : BaseController<CommissionRulesController>
    {
        private readonly ICommissionRuleAppService _commissionRuleAppService;

        public CommissionRulesController(
            ILogger<CommissionRulesController> logger,
            ICommissionRuleAppService commissionRuleAppService) : base(logger)
        {
            _commissionRuleAppService = commissionRuleAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CommissionRuleResponseDTO>>> GetAll(CancellationToken ct)
        {
            IReadOnlyList<CommissionRuleResponseDTO> response = await _commissionRuleAppService.GetAllAsync(ct);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CommissionRuleResponseDTO>> GetById(int id)
        {
            CommissionRuleResponseDTO response = await _commissionRuleAppService.GetResponseByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CommissionRuleResponseDTO>> Create([FromBody] CreateCommissionRuleRequestDTO request, CancellationToken ct)
        {
            CommissionRuleResponseDTO response = await _commissionRuleAppService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CommissionRuleResponseDTO>> Update(int id, [FromBody] UpdateCommissionRuleRequestDTO request)
        {
            CommissionRuleResponseDTO response = await _commissionRuleAppService.UpdateAsync(id, request);
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _commissionRuleAppService.DeleteAsync(id);
            return NoContent();
        }
    }
}

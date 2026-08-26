using API.Base;
using AppService.AppService.Interfaces;
using Crosscutting.DTO.Commission;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CommissionsController : BaseController<CommissionsController>
    {
        private readonly ICommissionAppService _commissionAppService;

        public CommissionsController(
            ILogger<CommissionsController> logger,
            ICommissionAppService commissionAppService) : base(logger)
        {
            _commissionAppService = commissionAppService;
        }

        [HttpGet("sale/{saleId:int}")]
        public async Task<ActionResult<IReadOnlyList<CommissionResponseDTO>>> GetBySale(int saleId, CancellationToken ct)
        {
            IReadOnlyList<CommissionResponseDTO> response = await _commissionAppService.GetBySaleIdAsync(saleId, ct);
            return Ok(response);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<ActionResult<CommissionResponseDTO>> UpdateStatus(int id, [FromBody] UpdateCommissionStatusRequestDTO request)
        {
            CommissionResponseDTO response = await _commissionAppService.UpdateStatusAsync(id, request);
            return Ok(response);
        }
    }
}

using API.Base;
using AppService.AppService.Interfaces;
using Crosscutting.DTO.Discount;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DiscountsController : BaseController<DiscountsController>
    {
        private readonly IDiscountAppService _discountAppService;

        public DiscountsController(
            ILogger<DiscountsController> logger,
            IDiscountAppService discountAppService) : base(logger)
        {
            _discountAppService = discountAppService;
        }

        [HttpPost("apply")]
        public async Task<ActionResult<DiscountResponseDTO>> Apply([FromBody] ApplyDiscountRequestDTO request, CancellationToken ct)
        {
            DiscountResponseDTO response = await _discountAppService.ApplyDiscountAsync(request, ct);
            return Ok(response);
        }

        [HttpGet("proposal/{proposalId:int}")]
        public async Task<ActionResult<IReadOnlyList<DiscountResponseDTO>>> GetByProposal(int proposalId, CancellationToken ct)
        {
            IReadOnlyList<DiscountResponseDTO> response = await _discountAppService.GetByProposalIdAsync(proposalId, ct);
            return Ok(response);
        }

        [HttpGet("sale/{saleId:int}")]
        public async Task<ActionResult<IReadOnlyList<DiscountResponseDTO>>> GetBySale(int saleId, CancellationToken ct)
        {
            IReadOnlyList<DiscountResponseDTO> response = await _discountAppService.GetBySaleIdAsync(saleId, ct);
            return Ok(response);
        }
    }
}

using API.Base;
using AppService.AppService.Interfaces;
using Crosscutting.DTO.Proposal;
using Crosscutting.DTO.Sale;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProposalsController : BaseController<ProposalsController>
    {
        private readonly IProposalAppService _proposalAppService;

        public ProposalsController(
            ILogger<ProposalsController> logger,
            IProposalAppService proposalAppService) : base(logger)
        {
            _proposalAppService = proposalAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProposalResponseDTO>>> GetAll(CancellationToken ct)
        {
            IReadOnlyList<ProposalResponseDTO> response = await _proposalAppService.GetAllAsync(ct);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProposalResponseDTO>> GetById(int id, CancellationToken ct)
        {
            ProposalResponseDTO response = await _proposalAppService.GetResponseByIdAsync(id, ct);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ProposalResponseDTO>> Create([FromBody] CreateProposalRequestDTO request, CancellationToken ct)
        {
            ProposalResponseDTO response = await _proposalAppService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProposalResponseDTO>> Update(int id, [FromBody] UpdateProposalRequestDTO request, CancellationToken ct)
        {
            ProposalResponseDTO response = await _proposalAppService.UpdateAsync(id, request, ct);
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _proposalAppService.DeleteAsync(id, ct);
            return NoContent();
        }

        [HttpPost("{id:int}/send")]
        public async Task<ActionResult<ProposalResponseDTO>> Send(int id, CancellationToken ct)
        {
            ProposalResponseDTO response = await _proposalAppService.SendAsync(id, ct);
            return Ok(response);
        }

        [HttpPost("{id:int}/approve")]
        public async Task<ActionResult<ProposalResponseDTO>> Approve(int id, CancellationToken ct)
        {
            ProposalResponseDTO response = await _proposalAppService.ApproveAsync(id, ct);
            return Ok(response);
        }

        [HttpPost("{id:int}/convert-to-sale")]
        public async Task<ActionResult<SaleResponseDTO>> ConvertToSale(int id, CancellationToken ct)
        {
            SaleResponseDTO response = await _proposalAppService.ConvertToSaleAsync(id, ct);
            return CreatedAtAction(
                nameof(SalesController.GetById),
                "Sales",
                new { id = response.Id },
                response);
        }
    }
}

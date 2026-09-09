using API.Base;
using AppService.AppService.Interfaces;
using Crosscutting.DTO.Sale;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SalesController : BaseController<SalesController>
    {
        private readonly ISaleAppService _saleAppService;

        public SalesController(
            ILogger<SalesController> logger,
            ISaleAppService saleAppService) : base(logger)
        {
            _saleAppService = saleAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SaleResponseDTO>>> GetConfirmed(CancellationToken ct)
        {
            IReadOnlyList<SaleResponseDTO> response = await _saleAppService.GetConfirmedAsync(ct);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SaleResponseDTO>> GetById(int id, CancellationToken ct)
        {
            SaleResponseDTO response = await _saleAppService.GetResponseByIdAsync(id, ct);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<SaleResponseDTO>> Create([FromBody] CreateSaleRequestDTO request, CancellationToken ct)
        {
            SaleResponseDTO response = await _saleAppService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPost("{id:int}/confirm")]
        public async Task<ActionResult<SaleResponseDTO>> Confirm(int id, CancellationToken ct)
        {
            SaleResponseDTO response = await _saleAppService.ConfirmAsync(id, ct);
            return Ok(response);
        }
    }
}

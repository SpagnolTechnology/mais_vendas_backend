using API.Base;
using AppService.AppService.Interfaces;
using Crosscutting.DTO.StockAdjustment;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StockAdjustmentsController : BaseController<StockAdjustmentsController>
    {
        private readonly IStockAdjustmentAppService _stockAdjustmentAppService;

        public StockAdjustmentsController(
            ILogger<StockAdjustmentsController> logger,
            IStockAdjustmentAppService stockAdjustmentAppService) : base(logger)
        {
            _stockAdjustmentAppService = stockAdjustmentAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<StockAdjustmentResponseDTO>>> GetAll(CancellationToken ct)
        {
            IReadOnlyList<StockAdjustmentResponseDTO> response = await _stockAdjustmentAppService.GetAllAsync(ct);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<StockAdjustmentResponseDTO>> GetById(int id, CancellationToken ct)
        {
            StockAdjustmentResponseDTO response = await _stockAdjustmentAppService.GetResponseByIdAsync(id, ct);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<StockAdjustmentResponseDTO>> Create([FromBody] CreateStockAdjustmentRequestDTO request, CancellationToken ct)
        {
            StockAdjustmentResponseDTO response = await _stockAdjustmentAppService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<StockAdjustmentResponseDTO>> Update(int id, [FromBody] UpdateStockAdjustmentRequestDTO request, CancellationToken ct)
        {
            StockAdjustmentResponseDTO response = await _stockAdjustmentAppService.UpdateAsync(id, request, ct);
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _stockAdjustmentAppService.DeleteAsync(id, ct);
            return NoContent();
        }

        [HttpPost("{id:int}/confirm")]
        public async Task<ActionResult<StockAdjustmentResponseDTO>> Confirm(int id, CancellationToken ct)
        {
            await _stockAdjustmentAppService.ConfirmAsync(id, ct);
            StockAdjustmentResponseDTO response = await _stockAdjustmentAppService.GetResponseByIdAsync(id, ct);
            return Ok(response);
        }
    }
}

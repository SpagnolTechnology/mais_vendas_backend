using API.Base;
using AppService.AppService.Interfaces;
using Crosscutting.DTO.Product;
using Crosscutting.DTO.StockMovement;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseController<ProductsController>
    {
        private readonly IProductAppService _productAppService;

        public ProductsController(
            ILogger<ProductsController> logger,
            IProductAppService productAppService) : base(logger)
        {
            _productAppService = productAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductResponseDTO>>> GetAll(CancellationToken ct)
        {
            IReadOnlyList<ProductResponseDTO> response = await _productAppService.GetAllAsync(ct);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponseDTO>> GetById(int id)
        {
            ProductResponseDTO response = await _productAppService.GetResponseByIdAsync(id);
            return Ok(response);
        }

        [HttpGet("{id:int}/stock-summary")]
        public async Task<ActionResult<ProductStockSummaryResponseDTO>> GetStockSummary(int id, CancellationToken ct)
        {
            ProductStockSummaryResponseDTO response = await _productAppService.GetStockSummaryAsync(id, ct);
            return Ok(response);
        }

        [HttpGet("{id:int}/stock-movements")]
        public async Task<ActionResult<IReadOnlyList<StockMovementResponseDTO>>> GetStockMovements(int id, CancellationToken ct)
        {
            IReadOnlyList<StockMovementResponseDTO> response = await _productAppService.GetMovementsAsync(id, ct);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDTO>> Create([FromBody] CreateProductRequestDTO request, CancellationToken ct)
        {
            ProductResponseDTO response = await _productAppService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductResponseDTO>> Update(int id, [FromBody] UpdateProductRequestDTO request)
        {
            ProductResponseDTO response = await _productAppService.UpdateAsync(id, request);
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productAppService.DeleteAsync(id);
            return NoContent();
        }
    }
}

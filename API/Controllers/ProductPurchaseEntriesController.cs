using API.Base;
using AppService.AppService.Interfaces;
using Crosscutting.DTO.ProductPurchaseEntry;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductPurchaseEntriesController : BaseController<ProductPurchaseEntriesController>
    {
        private readonly IProductPurchaseEntryAppService _productPurchaseEntryAppService;

        public ProductPurchaseEntriesController(
            ILogger<ProductPurchaseEntriesController> logger,
            IProductPurchaseEntryAppService productPurchaseEntryAppService) : base(logger)
        {
            _productPurchaseEntryAppService = productPurchaseEntryAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductPurchaseEntryResponseDTO>>> GetAll(CancellationToken ct)
        {
            IReadOnlyList<ProductPurchaseEntryResponseDTO> response = await _productPurchaseEntryAppService.GetAllAsync(ct);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductPurchaseEntryResponseDTO>> GetById(int id, CancellationToken ct)
        {
            ProductPurchaseEntryResponseDTO response = await _productPurchaseEntryAppService.GetResponseByIdAsync(id, ct);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ProductPurchaseEntryResponseDTO>> Create([FromBody] CreateProductPurchaseEntryRequestDTO request, CancellationToken ct)
        {
            ProductPurchaseEntryResponseDTO response = await _productPurchaseEntryAppService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductPurchaseEntryResponseDTO>> Update(int id, [FromBody] UpdateProductPurchaseEntryRequestDTO request, CancellationToken ct)
        {
            ProductPurchaseEntryResponseDTO response = await _productPurchaseEntryAppService.UpdateAsync(id, request, ct);
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _productPurchaseEntryAppService.DeleteAsync(id, ct);
            return NoContent();
        }

        [HttpPost("{id:int}/confirm")]
        public async Task<ActionResult<ProductPurchaseEntryResponseDTO>> Confirm(int id, CancellationToken ct)
        {
            await _productPurchaseEntryAppService.ConfirmAsync(id, ct);
            ProductPurchaseEntryResponseDTO response = await _productPurchaseEntryAppService.GetResponseByIdAsync(id, ct);
            return Ok(response);
        }
    }
}

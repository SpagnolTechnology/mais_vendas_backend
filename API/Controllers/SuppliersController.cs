using API.Base;
using AppService.AppService.Interfaces;
using Crosscutting.DTO.Supplier;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SuppliersController : BaseController<SuppliersController>
    {
        private readonly ISupplierAppService _supplierAppService;

        public SuppliersController(
            ILogger<SuppliersController> logger,
            ISupplierAppService supplierAppService) : base(logger)
        {
            _supplierAppService = supplierAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SupplierResponseDTO>>> GetAll(CancellationToken ct)
        {
            IReadOnlyList<SupplierResponseDTO> response = await _supplierAppService.GetAllAsync(ct);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SupplierResponseDTO>> GetById(int id)
        {
            SupplierResponseDTO response = await _supplierAppService.GetResponseByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierResponseDTO>> Create([FromBody] CreateSupplierRequestDTO request, CancellationToken ct)
        {
            SupplierResponseDTO response = await _supplierAppService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<SupplierResponseDTO>> Update(int id, [FromBody] UpdateSupplierRequestDTO request)
        {
            SupplierResponseDTO response = await _supplierAppService.UpdateAsync(id, request);
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _supplierAppService.DeleteAsync(id);
            return NoContent();
        }
    }
}

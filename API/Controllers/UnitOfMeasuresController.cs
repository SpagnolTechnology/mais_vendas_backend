using API.Base;
using AppService.AppService.Interfaces;
using Crosscutting.DTO.UnitOfMeasure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UnitOfMeasuresController : BaseController<UnitOfMeasuresController>
    {
        private readonly IUnitOfMeasureAppService _unitOfMeasureAppService;

        public UnitOfMeasuresController(
            ILogger<UnitOfMeasuresController> logger,
            IUnitOfMeasureAppService unitOfMeasureAppService) : base(logger)
        {
            _unitOfMeasureAppService = unitOfMeasureAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UnitOfMeasureResponseDTO>>> GetAll(CancellationToken ct)
        {
            IReadOnlyList<UnitOfMeasureResponseDTO> response = await _unitOfMeasureAppService.GetAllAsync(ct);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UnitOfMeasureResponseDTO>> GetById(int id)
        {
            UnitOfMeasureResponseDTO response = await _unitOfMeasureAppService.GetResponseByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<UnitOfMeasureResponseDTO>> Create([FromBody] CreateUnitOfMeasureRequestDTO request, CancellationToken ct)
        {
            UnitOfMeasureResponseDTO response = await _unitOfMeasureAppService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<UnitOfMeasureResponseDTO>> Update(int id, [FromBody] UpdateUnitOfMeasureRequestDTO request)
        {
            UnitOfMeasureResponseDTO response = await _unitOfMeasureAppService.UpdateAsync(id, request);
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _unitOfMeasureAppService.DeleteAsync(id);
            return NoContent();
        }
    }
}

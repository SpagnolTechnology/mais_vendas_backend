using API.Base;
using AppService.AppService.Interfaces;
using Crosscutting.DTO.Test;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TestsController : BaseController<TestsController>
    {
        private readonly ITestAppService _testAppService;

        public TestsController(
            ILogger<TestsController> logger,
            ITestAppService testAppService) : base(logger)
        {
            _testAppService = testAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TestResponseDTO>>> GetAll(CancellationToken ct)
        {
            IReadOnlyList<TestResponseDTO> response = await _testAppService.GetAllAsync(ct);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TestResponseDTO>> GetById(int id)
        {
            TestResponseDTO response = await _testAppService.GetResponseByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<TestResponseDTO>> Create([FromBody] CreateTestRequestDTO request, CancellationToken ct)
        {
            TestResponseDTO response = await _testAppService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TestResponseDTO>> Update(int id, [FromBody] UpdateTestRequestDTO request)
        {
            TestResponseDTO response = await _testAppService.UpdateAsync(id, request);
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _testAppService.DeleteAsync(id);
            return NoContent();
        }
    }
}

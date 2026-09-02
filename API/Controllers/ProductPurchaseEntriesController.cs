using API.Base;
using AppService.AppService.Interfaces;
using Crosscutting.CustomException;
using Crosscutting.DTO.NfeImport;
using Crosscutting.DTO.ProductPurchaseEntry;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductPurchaseEntriesController : BaseController<ProductPurchaseEntriesController>
    {
        private readonly IProductPurchaseEntryAppService _productPurchaseEntryAppService;
        private readonly INfeImportAppService _nfeImportAppService;

        public ProductPurchaseEntriesController(
            ILogger<ProductPurchaseEntriesController> logger,
            IProductPurchaseEntryAppService productPurchaseEntryAppService,
            INfeImportAppService nfeImportAppService) : base(logger)
        {
            _productPurchaseEntryAppService = productPurchaseEntryAppService;
            _nfeImportAppService = nfeImportAppService;
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

        [HttpPost("import-xml/preview")]
        public async Task<ActionResult<NfeImportPreviewResponseDTO>> PreviewImport([FromBody] NfeImportPreviewRequestDTO request, CancellationToken ct)
        {
            NfeImportPreviewResponseDTO response = await _nfeImportAppService.PreviewAsync(request.XmlContent, ct);
            return Ok(response);
        }

        [HttpPost("import-xml/preview/upload")]
        public async Task<ActionResult<NfeImportPreviewResponseDTO>> PreviewImportUpload([FromForm] IFormFile file, CancellationToken ct)
        {
            string xmlContent = await ReadXmlFromFileAsync(file);
            NfeImportPreviewResponseDTO response = await _nfeImportAppService.PreviewAsync(xmlContent, ct);
            return Ok(response);
        }

        [HttpPost("import-xml/confirm")]
        public async Task<ActionResult<ProductPurchaseEntryResponseDTO>> ConfirmImport([FromBody] NfeImportConfirmRequestDTO request, CancellationToken ct)
        {
            ProductPurchaseEntryResponseDTO response = await _nfeImportAppService.ConfirmImportAsync(request, ct);
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

        private static async Task<string> ReadXmlFromFileAsync(IFormFile file)
        {
            if (file is null || file.Length == 0)
                throw new CustomBusinessException("Ops... O arquivo XML é obrigatório.");

            using StreamReader reader = new(file.OpenReadStream());
            return await reader.ReadToEndAsync();
        }
    }
}

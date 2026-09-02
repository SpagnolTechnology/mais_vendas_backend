using Crosscutting.DTO.NfeImport;
using Crosscutting.DTO.ProductPurchaseEntry;

namespace AppService.AppService.Interfaces
{
    public interface INfeImportAppService
    {
        Task<NfeImportPreviewResponseDTO> PreviewAsync(string xmlContent, CancellationToken ct = default);
        Task<ProductPurchaseEntryResponseDTO> ConfirmImportAsync(NfeImportConfirmRequestDTO request, CancellationToken ct = default);
    }
}

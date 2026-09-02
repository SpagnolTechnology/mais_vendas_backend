using Crosscutting.Enum;

namespace Crosscutting.DTO.NfeImport
{
    public class NfeImportItemPreviewDTO
    {
        public int LineNumber { get; set; }
        public string XmlProductCode { get; set; } = string.Empty;
        public string? XmlEan { get; set; }
        public string XmlProductName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public NfeImportItemMatchStatusEnum MatchStatus { get; set; }
        public int? MatchedProductId { get; set; }
        public string? MatchedProductName { get; set; }
        public decimal? DefaultMarkupPercent { get; set; }
    }
}

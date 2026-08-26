using Crosscutting.DTO.Base;
using Crosscutting.Enum;

namespace Crosscutting.DTO.Commission
{
    public class CommissionResponseDTO : BaseDTO
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int? SaleItemId { get; set; }
        public CommissionCalculationScopeEnum CalculationScope { get; set; }
        public string SellerEmail { get; set; } = string.Empty;
        public decimal CommissionPercent { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal CommissionAmount { get; set; }
        public CommissionStatusEnum Status { get; set; }
    }
}

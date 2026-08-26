using Domain.Base;

namespace Domain.Entity
{
    public class PaymentConditionEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int InstallmentCount { get; set; }
        public int DaysUntilFirstDue { get; set; }
        public int DaysBetweenInstallments { get; set; }
        public decimal CashDiscountPercent { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

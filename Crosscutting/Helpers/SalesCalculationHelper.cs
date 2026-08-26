namespace Crosscutting.Helpers
{
    public static class SalesCalculationHelper
    {
        public static decimal CalculateLineSubtotal(decimal quantity, decimal unitPrice, decimal discountPercent, decimal discountAmount)
        {
            decimal gross = quantity * unitPrice;
            decimal percentDiscount = gross * discountPercent / 100m;
            return Math.Max(0, gross - percentDiscount - discountAmount);
        }

        public static decimal CalculateTaxAmount(decimal lineSubtotal, decimal taxPercent)
        {
            return lineSubtotal * taxPercent / 100m;
        }

        public static decimal CalculateLineTotal(
            decimal lineSubtotal,
            decimal icmsAmount,
            decimal issAmount,
            decimal pisAmount,
            decimal cofinsAmount)
        {
            return lineSubtotal + icmsAmount + issAmount + pisAmount + cofinsAmount;
        }

        public static decimal CalculateUnitPriceFromCost(decimal costPrice, decimal markupPercent)
        {
            return costPrice * (1 + markupPercent / 100m);
        }

        public static decimal CalculateCommissionAmount(decimal baseAmount, decimal commissionPercent)
        {
            return baseAmount * commissionPercent / 100m;
        }
    }
}

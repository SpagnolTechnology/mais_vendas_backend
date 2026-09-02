namespace Crosscutting.Helpers
{
    public static class DocumentHelper
    {
        public static string NormalizeDigits(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return new string(value.Where(char.IsDigit).ToArray());
        }

        public static bool IsValidNfeEan(string? ean)
        {
            if (string.IsNullOrWhiteSpace(ean))
                return false;

            string trimmed = ean.Trim();
            if (trimmed.Equals("SEM GTIN", StringComparison.OrdinalIgnoreCase))
                return false;

            string digits = NormalizeDigits(trimmed);
            if (string.IsNullOrEmpty(digits))
                return false;

            return digits.Length >= 8;
        }
    }
}

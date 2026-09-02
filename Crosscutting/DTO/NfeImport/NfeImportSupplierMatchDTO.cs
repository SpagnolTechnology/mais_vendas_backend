namespace Crosscutting.DTO.NfeImport
{
    public class NfeImportSupplierMatchDTO
    {
        public int? MatchedSupplierId { get; set; }
        public NfeSuggestedSupplierDTO? SuggestedSupplier { get; set; }
    }
}

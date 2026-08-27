using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    [Table("ProposalAuthentication")]
    public class ProposalAuthenticationEntity
    {
        [Key]
        public string ProposalUuid { get; set; } = string.Empty;

        public string CompanyCnpj { get; set; } = string.Empty;
    }
}

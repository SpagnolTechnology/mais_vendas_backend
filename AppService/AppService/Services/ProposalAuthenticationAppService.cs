using AppService.AppService.Interfaces;
using AutoMapper;
using Domain.Entity;
using Infrastructure.Repository.Interfaces;

namespace AppService.AppService.Services
{
    public class ProposalAuthenticationAppService : BaseService<IProposalAuthenticationRepository, ProposalAuthenticationEntity>, IProposalAuthenticationAppService
    {
        public ProposalAuthenticationAppService(
            IMapper mapper,
            IProposalAuthenticationRepository repository) : base(mapper, repository)
        {
        }

        public async Task RegisterAsync(Guid proposalUuid, string companyCnpj, CancellationToken ct = default)
        {
            string normalizedCnpj = NormalizeDocument(companyCnpj);

            ProposalAuthenticationEntity entity = new()
            {
                ProposalUuid = proposalUuid.ToString(),
                CompanyCnpj = normalizedCnpj
            };

            await AddAsync(entity, ct);
        }

        public async Task<string?> GetCompanyCnpjByProposalUuidAsync(Guid proposalUuid, CancellationToken ct = default)
        {
            ProposalAuthenticationEntity? entity = await _repository.GetByProposalUuidAsync(proposalUuid.ToString(), ct);
            return entity?.CompanyCnpj;
        }

        private static string NormalizeDocument(string document)
        {
            return new string(document.Where(char.IsDigit).ToArray());
        }
    }
}

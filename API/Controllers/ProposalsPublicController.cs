using AppService.AppService.Interfaces;
using Crosscutting.CustomException;
using Infrastructure.Tenant.Provider;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("Proposals/public")]
    public class ProposalsPublicController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IProposalAuthenticationAppService _proposalAuthenticationAppService;
        private readonly ITenantContext _tenantContext;

        public ProposalsPublicController(
            IServiceProvider serviceProvider,
            IProposalAuthenticationAppService proposalAuthenticationAppService,
            ITenantContext tenantContext)
        {
            _serviceProvider = serviceProvider;
            _proposalAuthenticationAppService = proposalAuthenticationAppService;
            _tenantContext = tenantContext;
        }

        [HttpPost("{proposalUuid:guid}/approve")]
        public async Task<IActionResult> Approve(Guid proposalUuid, CancellationToken ct)
        {
            await ApplyTenantByProposalUuidAsync(proposalUuid, ct);

            IProposalAppService proposalAppService = _serviceProvider.GetRequiredService<IProposalAppService>();
            var response = await proposalAppService.ApproveByProposalUuidAsync(proposalUuid, ct);

            return Ok(response);
        }

        [HttpPost("{proposalUuid:guid}/reject")]
        public async Task<IActionResult> Reject(Guid proposalUuid, CancellationToken ct)
        {
            await ApplyTenantByProposalUuidAsync(proposalUuid, ct);

            IProposalAppService proposalAppService = _serviceProvider.GetRequiredService<IProposalAppService>();
            var response = await proposalAppService.RejectByProposalUuidAsync(proposalUuid, ct);

            return Ok(response);
        }

        private async Task ApplyTenantByProposalUuidAsync(Guid proposalUuid, CancellationToken ct)
        {
            string? companyCnpj = await _proposalAuthenticationAppService.GetCompanyCnpjByProposalUuidAsync(proposalUuid, ct);

            if (string.IsNullOrWhiteSpace(companyCnpj))
                throw new CustomBusinessException("Ops... Não foi possível identificar o tenant da proposta.", HttpStatusCode.NotFound);

            _tenantContext.SetCurrentCnpj(companyCnpj);
        }
    }
}

using Crosscutting.Configuration;
using Crosscutting.Constant;
using Crosscutting.CustomException;
using Crosscutting.DTO.External;
using Crosscutting.External;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace AppService.AppService.External
{
    public class ExternalBillingService : IExternalBillingService
    {
        private readonly HttpClient _httpClient;
        private readonly DomainsOptions _domainsOptions;
        private readonly ILogger<ExternalBillingService> _logger;

        public ExternalBillingService(
            HttpClient httpClient,
            IOptions<DomainsOptions> domainsOptions,
            ILogger<ExternalBillingService> logger)
        {
            _httpClient = httpClient;
            _domainsOptions = domainsOptions.Value;
            _logger = logger;
        }

        public async Task<string> CreateBillingAsync(CreateExternalBillingRequestDTO request, CancellationToken ct = default)
        {
            if (request.SaleId <= 0)
                throw new CustomBusinessException("Ops... Identificador da venda é obrigatório para gerar cobrança.");

            if (string.IsNullOrWhiteSpace(request.ExternalClientId))
                throw new CustomBusinessException("Ops... Cliente externo é obrigatório para gerar cobrança.");

            string url = BuildUrl(ExternalApiPaths.Billings);

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, request, ct);
                response.EnsureSuccessStatusCode();

                ExternalBillingResponseDTO? billing = await response.Content.ReadFromJsonAsync<ExternalBillingResponseDTO>(cancellationToken: ct);
                if (billing == null || string.IsNullOrWhiteSpace(billing.Id))
                    throw new CustomBusinessException("Ops... Resposta inválida ao gerar cobrança externa.");

                return billing.Id;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Falha ao gerar cobrança externa em {Url}. Utilizando mock.", url);
                return BuildMockBillingId(request);
            }
            catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
            {
                _logger.LogWarning(ex, "Timeout ao gerar cobrança externa em {Url}. Utilizando mock.", url);
                return BuildMockBillingId(request);
            }
        }

        private string BuildUrl(string path)
        {
            return $"{_domainsOptions.MaisLocacoesDomain.TrimEnd('/')}/{path.TrimStart('/')}";
        }

        private static string BuildMockBillingId(CreateExternalBillingRequestDTO request)
        {
            return $"MOCK-BILLING-{request.SaleId}-{DateTime.UtcNow:yyyyMMddHHmmss}";
        }
    }
}

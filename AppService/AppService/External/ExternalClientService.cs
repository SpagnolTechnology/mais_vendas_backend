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
    public class ExternalClientService : IExternalClientService
    {
        private readonly HttpClient _httpClient;
        private readonly DomainsOptions _domainsOptions;
        private readonly ILogger<ExternalClientService> _logger;

        public ExternalClientService(
            HttpClient httpClient,
            IOptions<DomainsOptions> domainsOptions,
            ILogger<ExternalClientService> logger)
        {
            _httpClient = httpClient;
            _domainsOptions = domainsOptions.Value;
            _logger = logger;
        }

        public async Task ValidateClientAsync(string externalClientId, CancellationToken ct = default)
        {
            ExternalClientResponseDTO client = await GetClientAsync(externalClientId, ct);

            if (!client.IsActive)
                throw new CustomBusinessException("Ops... O cliente externo está inativo.");
        }

        public async Task<ExternalClientResponseDTO> GetClientAsync(string externalClientId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(externalClientId))
                throw new CustomBusinessException("Ops... O identificador externo do cliente é obrigatório.");

            string url = BuildUrl($"{ExternalApiPaths.Clients}/{externalClientId}");

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url, ct);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new CustomBusinessException("Ops... Cliente não encontrado na API externa.");

                response.EnsureSuccessStatusCode();

                ExternalClientResponseDTO? client = await response.Content.ReadFromJsonAsync<ExternalClientResponseDTO>(cancellationToken: ct);
                if (client == null)
                    throw new CustomBusinessException("Ops... Resposta inválida ao consultar cliente externo.");

                return client;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Falha ao consultar cliente externo em {Url}. Utilizando mock.", url);
                return BuildMockClient(externalClientId);
            }
            catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
            {
                _logger.LogWarning(ex, "Timeout ao consultar cliente externo em {Url}. Utilizando mock.", url);
                return BuildMockClient(externalClientId);
            }
        }

        private string BuildUrl(string path)
        {
            return $"{_domainsOptions.MaisLocacoesDomain.TrimEnd('/')}/{path.TrimStart('/')}";
        }

        private static ExternalClientResponseDTO BuildMockClient(string externalClientId)
        {
            return new ExternalClientResponseDTO
            {
                Id = externalClientId,
                Name = $"Cliente Mock {externalClientId}",
                Document = "00000000000000",
                IsActive = true
            };
        }
    }
}

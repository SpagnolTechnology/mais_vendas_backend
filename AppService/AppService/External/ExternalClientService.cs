using Crosscutting.Configuration;
using Crosscutting.Constant;
using Crosscutting.CustomException;
using Crosscutting.DTO.External;
using Crosscutting.External;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace AppService.AppService.External
{
    public class ExternalClientService : IExternalClientService
    {
        private const string InactiveStatus = "inactive";
        private const string BlockedStatus = "blocked";

        private readonly HttpClient _httpClient;
        private readonly DomainsOptions _domainsOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ExternalClientService> _logger;

        public ExternalClientService(
            HttpClient httpClient,
            IOptions<DomainsOptions> domainsOptions,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ExternalClientService> logger)
        {
            _httpClient = httpClient;
            _domainsOptions = domainsOptions.Value;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task ValidateClientAsync(string externalClientId, CancellationToken ct = default)
        {
            ExternalClientResponseDTO client = await GetClientAsync(externalClientId, ct);

            if (string.Equals(client.Status, InactiveStatus, StringComparison.OrdinalIgnoreCase))
                throw new CustomBusinessException("Ops... O cliente está inativo.");

            if (string.Equals(client.Status, BlockedStatus, StringComparison.OrdinalIgnoreCase))
                throw new CustomBusinessException("Ops... O cliente está bloqueado.");
        }

        public async Task<ExternalClientResponseDTO> GetClientAsync(string externalClientId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(externalClientId))
                throw new CustomBusinessException("Ops... O identificador externo do cliente é obrigatório.");

            if (!int.TryParse(externalClientId, out int clientId) || clientId <= 0)
                throw new CustomBusinessException("Ops... O identificador do cliente é inválido.");

            string url = BuildUrl(ExternalApiPaths.ClientById(clientId));

            try
            {
                using HttpRequestMessage request = new(HttpMethod.Get, url);
                ApplyAuthorization(request);

                HttpResponseMessage response = await _httpClient.SendAsync(request, ct);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new CustomBusinessException("Ops... Cliente não encontrado na API de locações.");

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new CustomBusinessException("Ops... Não foi possível autenticar na API de locações.");

                response.EnsureSuccessStatusCode();

                ExternalClientResponseDTO? client = await response.Content.ReadFromJsonAsync<ExternalClientResponseDTO>(cancellationToken: ct);
                if (client == null || client.Id <= 0)
                    throw new CustomBusinessException("Ops... Resposta inválida ao consultar cliente na API de locações.");

                return client;
            }
            catch (CustomBusinessException)
            {
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Falha ao consultar cliente externo em {Url}.", url);
                throw new CustomBusinessException("Ops... Não foi possível validar o cliente na API de locações.");
            }
            catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
            {
                _logger.LogWarning(ex, "Timeout ao consultar cliente externo em {Url}.", url);
                throw new CustomBusinessException("Ops... Não foi possível validar o cliente na API de locações.");
            }
        }

        private void ApplyAuthorization(HttpRequestMessage request)
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            if (httpContext is null)
                return;

            if (httpContext.Request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues authorizationHeader))
                request.Headers.TryAddWithoutValidation("Authorization", authorizationHeader.ToString());
        }

        private string BuildUrl(string path)
        {
            return $"{_domainsOptions.MaisLocacoesDomain.TrimEnd('/')}/{path.TrimStart('/')}";
        }
    }
}

using Crosscutting.CustomException;
using Crosscutting.DTO.CustomException;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        private const string MESSAGE = "Houve um erro inesperado no servidor.";
        private const string CONTENT_TYPE = "application/json; charset=utf-8";

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (CustomBusinessException ex)
            {
                await HandleCustomExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleCustomExceptionAsync(HttpContext context, CustomBusinessException customException)
        {
            BaseExceptionAsync(context, customException, customException.Status);

            await SetResponseError(context, customException.Message);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            BaseExceptionAsync(context, exception);

            await SetResponseError(context, MESSAGE);
        }

        private void BaseExceptionAsync(HttpContext context, Exception exception, HttpStatusCode? statusCode = HttpStatusCode.InternalServerError)
        {
            context.Response.ContentType = CONTENT_TYPE;
            context.Response.StatusCode = statusCode != null ? (int)statusCode : (int)HttpStatusCode.InternalServerError;

            _logger.LogError(exception, exception.Message);
        }

        private static async Task SetResponseError(HttpContext context, string message)
        {
            IReadOnlyCollection<string> messages = message.Split('|');
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ExceptionResponseDTO(messages)));
        }
    }
}

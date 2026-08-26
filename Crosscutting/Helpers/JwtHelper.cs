using Crosscutting.CustomException;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Crosscutting.Helpers
{
    public class JwtHelper
    {
        private const string BearerPrefix = "Bearer ";
        private const string MaisLocacoesJwtSecret = "%ATpGhSb9Xy3@6uWEfZmmz%bbg^XxVe4hDa3!J$4^jLsCBBrpbR!LoU9uLb^XpUQc2yEaDWRK9#JRQ@wu@QbcsFnvb!%b8To#sQE%ivbpvYAEoR8p$iFK@nNDur5@MSzbY&86A$UrUy$GDhdwz^AW6Cz^aSbz2YhWFCwVF8Nd2D8LssKn#pSfbY7oD9HzGa&AQJsnEdgx!Z4wJ3UVf2i@RVDt2c@6Y8xWHg%MY2sns8wELSXsHvitNXMxowtG@kx6obzruFu%eeNcTRkpsd6^HM%UkC9BbB522X4LXinh6nn4HqMY&HDtwAK!^6YHBPHjhqA47m8erDseP23FGJ#MCZi%6xCVEjd72mF@W#xMC7bnbjW%SDAp4Rs3pXRJz&#@oVj7FrhZdhXiytUbUyRUS^hRS^MGZ75@@dhK6y7gcNYW2Sfj@JM$o7qxmwTp!nNdwCgd43zB9$CU&%NM#pd9N%e2wAH3kzAWAT8^xJ2QqzSWoQp3WGeZt5dk!#FMg&b";

        public static ClaimsPrincipal ValidateToken(string token)
        {
            return ValidateToken(token, MaisLocacoesJwtSecret);
        }

        public static ClaimsPrincipal ValidateToken(string token, string secret)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw Unauthorized("Token não informado.");

            if (string.IsNullOrWhiteSpace(secret))
                throw new CustomBusinessException("Chave de validação JWT não configurada.", HttpStatusCode.InternalServerError);

            byte[] key = Encoding.ASCII.GetBytes(secret);
            SymmetricSecurityKey securityKey = new(key);

            JwtSecurityTokenHandler tokenHandler = new();

            TokenValidationParameters parameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ClockSkew = TimeSpan.Zero,
                NameClaimType = ClaimTypes.Name,
                RoleClaimType = ClaimTypes.Role
            };

            try
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, parameters, out SecurityToken validatedToken);

                if (validatedToken is not JwtSecurityToken jwtToken || !IsExpectedAlgorithm(jwtToken.Header.Alg))
                    throw Unauthorized("Algoritmo de assinatura do token inválido.");

                return claimsPrincipal;
            }
            catch (CustomBusinessException)
            {
                throw;
            }
            catch (SecurityTokenExpiredException ex)
            {
                throw Unauthorized("Token expirado.", ex);
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
                throw Unauthorized("Assinatura do token inválida.", ex);
            }
            catch (SecurityTokenException ex)
            {
                throw Unauthorized("Token inválido.", ex);
            }
            catch (ArgumentException ex)
            {
                throw Unauthorized("Token inválido.", ex);
            }
        }

        public static string ExtractTokenByAuthorization(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext is null)
                throw Unauthorized("Contexto HTTP não encontrado.");

            return ExtractTokenByAuthorization(httpContextAccessor.HttpContext);
        }

        public static string ExtractTokenByAuthorization(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues authorizationHeader))
                throw Unauthorized("Token não informado.");

            string authorization = authorizationHeader.ToString();

            if (string.IsNullOrWhiteSpace(authorization) || !authorization.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase))
                throw Unauthorized("Header Authorization deve usar o formato 'Bearer {token}'.");

            string token = authorization[BearerPrefix.Length..].Trim();

            if (string.IsNullOrWhiteSpace(token))
                throw Unauthorized("Token não informado.");

            return token;
        }

        public static string ExtractPropertyByToken(string token, string property)
        {
            string? value = TryExtractPropertyByToken(token, property);

            if (value is not null)
                return value;

            throw Unauthorized($"Propriedade '{property}' não encontrada no token.");
        }

        public static string? TryExtractPropertyByToken(string token, string property)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw Unauthorized("Token não informado.");

            if (string.IsNullOrWhiteSpace(property))
                throw Unauthorized("Propriedade do token não informada.");

            JwtSecurityTokenHandler handler = new();

            try
            {
                JwtSecurityToken decodedToken = handler.ReadJwtToken(token);

                if (!decodedToken.Payload.TryGetValue(property, out object? value) || value is null)
                    return null;

                return value.ToString();
            }
            catch (ArgumentException ex)
            {
                throw Unauthorized("Token inválido.", ex);
            }
        }

        public static string GetEmailByToken(IHttpContextAccessor httpContextAccessor)
        {
            string token = ExtractTokenByAuthorization(httpContextAccessor);
            return ExtractPropertyByToken(token, "email");
        }

        public static string GetCnpjByToken(IHttpContextAccessor httpContextAccessor)
        {
            string token = ExtractTokenByAuthorization(httpContextAccessor);
            return ExtractPropertyByToken(token, "cnpj");
        }

        public static string GetCpfByToken(IHttpContextAccessor httpContextAccessor)
        {
            string token = ExtractTokenByAuthorization(httpContextAccessor);
            return ExtractPropertyByToken(token, "cpf");
        }

        public static string GetRoleByToken(IHttpContextAccessor httpContextAccessor)
        {
            string token = ExtractTokenByAuthorization(httpContextAccessor);
            return ExtractPropertyByToken(token, "role");
        }

        public static string GetTimeZoneByToken(IHttpContextAccessor httpContextAccessor)
        {
            string token = ExtractTokenByAuthorization(httpContextAccessor);
            return ExtractPropertyByToken(token, "timeZone");
        }

        public static string GetUserNameByToken(IHttpContextAccessor httpContextAccessor)
        {
            string token = ExtractTokenByAuthorization(httpContextAccessor);
            return ExtractPropertyByToken(token, "unique_name");
        }

        private static bool IsExpectedAlgorithm(string? algorithm)
        {
            return string.Equals(algorithm, SecurityAlgorithms.HmacSha256, StringComparison.Ordinal)
                || string.Equals(algorithm, SecurityAlgorithms.HmacSha256Signature, StringComparison.Ordinal);
        }

        private static CustomBusinessException Unauthorized(string message)
        {
            return new CustomBusinessException(message, HttpStatusCode.Unauthorized);
        }

        private static CustomBusinessException Unauthorized(string message, Exception innerException)
        {
            return new CustomBusinessException(message, innerException, HttpStatusCode.Unauthorized);
        }
    }
}

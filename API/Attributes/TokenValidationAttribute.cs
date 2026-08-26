using Crosscutting.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace API.Attributes
{
    public class TokenValidationAttribute : TypeFilterAttribute
    {
        public TokenValidationAttribute() : base(typeof(TokenValidationFilter))
        {

        }

        private class TokenValidationFilter : IAsyncAuthorizationFilter
        {
            public Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                string token = JwtHelper.ExtractTokenByAuthorization(context.HttpContext);
                ClaimsPrincipal validatedToken = JwtHelper.ValidateToken(token);

                context.HttpContext.User = validatedToken;
                return Task.CompletedTask;
            }
        }
    }
}

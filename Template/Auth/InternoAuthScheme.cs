using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Template.Auth
{
    public class InternoAuthSchemeOptions : AuthenticationSchemeOptions
    {
    }
    public class InternoAuthScheme(
    IOptionsMonitor<InternoAuthSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) : AuthenticationHandler<InternoAuthSchemeOptions>(options, logger, encoder)
    {
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Aplicar la misma logica que se usó en ExternoAuthScheme

            var claims = new[] { new Claim(ClaimTypes.Name, "Usuario Desconocido") };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer Token"));
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}

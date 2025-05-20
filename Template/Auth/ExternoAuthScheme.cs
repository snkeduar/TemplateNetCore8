using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using APITerceros.SeguridadApi;

namespace Template.Auth
{
    public class ExternoAuthSchemeOptions : AuthenticationSchemeOptions
    {
    }
    public class ExternoAuthScheme : AuthenticationHandler<ExternoAuthSchemeOptions>
    {
        private readonly ISeguridadService _seguridadService;
        public ExternoAuthScheme(
        IOptionsMonitor<ExternoAuthSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder, ISeguridadService seguridadService) : base(options, logger, encoder) => _seguridadService = seguridadService;
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorizationBearer = Request.Headers.Authorization.FirstOrDefault();

            if (authorizationBearer == string.Empty || authorizationBearer is null || authorizationBearer.Split(" ").Length < 2)
            {
                return AuthenticateResult.Fail("Authorization Bearer Token is not valid");
            }

            string token = authorizationBearer.Split(" ")[1];

            if (await _seguridadService.ValidateClientToken(token))
            {
                var claims = new[] { new Claim(ClaimTypes.Name, "Usuario Desconocido") };
                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer Token"));
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            return AuthenticateResult.Fail("No authenticated");
        }
    }
}

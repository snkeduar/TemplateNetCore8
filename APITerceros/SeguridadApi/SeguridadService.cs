using CommonServices;
using Microsoft.Extensions.Configuration;
using Models.APITerceros;
using Models.Auth;

namespace APITerceros.SeguridadApi
{
    public interface ISeguridadService
    {
        Task<bool> ValidateClientToken(string token);
    }
    public class SeguridadService : ISeguridadService
    {
        private readonly IRestClientService _restClientService;
        private readonly IUtilsService _utilsService;
        public SeguridadService(IRestClientService restClientService, IConfiguration configuration, IUtilsService utilsService)
        {
            _utilsService = utilsService;
            _restClientService = restClientService;
            _restClientService.SetBaseUri(configuration.GetSection("Apis").GetValue<string>("SeguridadApi"));
        }

        public async Task<bool> ValidateClientToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            ClientTokenValidationObject tokenRequest = new() { Token = token };

            ClientTokenValidationRequest request = new() { Base64 = _utilsService.ToBase64(tokenRequest) };

            var restClientResponse = await _restClientService.Post<bool, dynamic>(request, EnumBodyType.ApplicationJson, "validateTokenProvider");

            return restClientResponse.Data;
        }
    }
}

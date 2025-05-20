using System.Text.Json.Serialization;

namespace Models.Auth
{
    public class ClientTokenValidationObject
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = "";
    }
}

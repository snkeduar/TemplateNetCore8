using System.Text.Json.Serialization;

namespace Models.Auth
{
    public class ClientTokenValidationRequest
    {
        [JsonPropertyName("base64")]
        public string Base64 { get; set; } = "";
    }
}


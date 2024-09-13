using System.Text.Json.Serialization;

namespace Models.External.AtProto.Server
{
    public class CreateSessionRequest
    {
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
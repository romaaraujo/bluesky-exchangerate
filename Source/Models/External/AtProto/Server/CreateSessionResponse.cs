using System.Text.Json.Serialization;

namespace Models.External.AtProto.Server
{
    public class CreateSessionResponse
    {
        [JsonPropertyName("accessJwt")]
        public string AccessJwt { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("refreshJwt")]
        public string RefreshJwt { get; set; }

        [JsonPropertyName("emailConfirmed")]
        public bool EmailConfirmed { get; set; }


    }
}
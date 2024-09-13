using System.Text.Json.Serialization;

namespace Models.External.AtProto.Repo
{
    public class BaseRequest
    {
        [JsonPropertyName("repo")]
        public string Repo { get; set; }

        [JsonPropertyName("collection")]
        public string Collection { get; set; }
    }
}
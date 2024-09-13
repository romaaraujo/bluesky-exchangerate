using System.Text.Json.Serialization;

namespace Models.External.AtProto.Repo
{
    public class CreateRecordResponse
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("cid")]
        public string Cid { get; set; }

        [JsonPropertyName("validationStatus")]
        public string ValidationStatus { get; set; }
    }
}

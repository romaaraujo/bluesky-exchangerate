using System.Text.Json.Serialization;

namespace Models.External.AtProto.Repo
{
    public class CreateRecordRequest : BaseRequest
    {
        [JsonPropertyName("record")]
        public Record Record { get; set; }
    }

    public class Record
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
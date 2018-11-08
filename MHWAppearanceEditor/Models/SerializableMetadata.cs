using Newtonsoft.Json;

namespace MHWAppearanceEditor.Models
{
    public class SerializableMetadata
    {
        [JsonProperty("Cirilla_PreviewImage")]
        public string PreviewImage { get; set; }

        [JsonProperty("Cirilla_Title")]
        public string Title { get; set; }

        [JsonProperty("Cirilla_Author")]
        public string Author { get; set; }

        [JsonProperty("Cirilla_Website")]
        public string Website { get; set; }

        [JsonProperty("Cirilla_Description")]
        public string Description { get; set; }
    }
}

using Newtonsoft.Json;

namespace LayeredAPI.Domain.Models.Request;

public class ImageRequest
{
    [JsonProperty("base64Image")]
    public string Base64Image { get; set; }
    
    [JsonProperty("extension")]
    public string Extension { get; set; }
}
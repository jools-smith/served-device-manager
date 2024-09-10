using System.Text.Json.Serialization;
using static SDM.Client;

namespace SDM {
  public partial class Response {
    public class Preview {
      [JsonPropertyName("requestHostId")]
      public Host? HostId { get; set; }
      [JsonPropertyName("features")]
      public List<Feature>? Features { get; set; }
      [JsonPropertyName("statusList")]
      public List<Status>? Statuses { get; set; }
    }
  }
}

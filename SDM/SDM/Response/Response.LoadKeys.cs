using System.Text.Json.Serialization;

namespace SDM {
  public partial class Response {
    public class LoadKeys {
      [JsonPropertyName("publicKey")]
      public byte[] PublicKey { get; set; } = [];
    }
  }
}

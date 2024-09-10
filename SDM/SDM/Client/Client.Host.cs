using System.Text.Json.Serialization;

namespace SDM {

  public partial class Client {
    public class Host {
      [JsonPropertyName("type")]
      public string Type { get; } 

      [JsonPropertyName("value")]
      public string Value { get; }

      internal Host(string type, string value)  {
        Type = type;
        Value = value;
      }

      public Host() => Type = Value = string.Empty;

    }
  }
}

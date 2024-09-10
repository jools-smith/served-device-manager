using System.Text.Json.Serialization;

namespace SDM {
  public partial class Request {
    public class Feature {
      public string? name { get; set; }
      public string? version { get; set; }
      public long? count { get; set; }
    }
  }
}

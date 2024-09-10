using System.Text.Json.Serialization;

namespace SDM {
  public partial class Response {
    public class Authorize {
      public DateTime expires { get; set; } = DateTime.MinValue;

      public string token { get; set; } = "";
    }
  }
}

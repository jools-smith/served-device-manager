using System.Text.Json.Serialization;

namespace SDM {
  public partial class Request {
    public class Authorize {
      public string user { get; private set; } = "";
      public string password { get; private set; } = "";

      public static Authorize Create => new();
      public Authorize WithUserName(string value) {
        this.user = value;
        return this;
      }
      public Authorize WithPassword(string value) {
        this.password = value;
        return this;
      }
    }
  }
}

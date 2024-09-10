using System.Text.Json.Serialization;
using static SDM.Response;

namespace SDM {

  public partial class Client {
    public class Session {
      public string Token { get; internal set; } = "";

    }
  }
}

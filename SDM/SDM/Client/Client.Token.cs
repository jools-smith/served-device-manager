using System.Text.Json.Serialization;

namespace SDM {

  public partial class Client {
    public class Token {
      public string Jwt { get; }

      internal Token(string value) => Jwt = value;

      public Token() => Jwt = string.Empty;
    }
  }
}

using RestSharp;
using static SDM.Response;

namespace SDM {

  public partial class Client {
    public class LoadPublicKeyBuilder : AbstractBuilder {

      public byte[]? PublicKey { get; internal set; }
      public string? Token { get; internal set; }

      internal LoadPublicKeyBuilder(string url) : base(url) {
      }

      public LoadPublicKeyBuilder WithSession(Session value) {
        Token = value.Token;
        return this;
      }
      public LoadPublicKeyBuilder WithPublicKeyDer(byte[] value) {
        PublicKey = value;
        return this;
      }


      public LoadKeys Execute() {
        var request = new RestSharp.RestRequest {
          Method = Method.Post,
          Resource = "/rest_licensing_keys",
          RequestFormat = DataFormat.Xml
        };

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/octet-stream");
        request.AddHeader("Authorization", $"Bearer {AssertNotNull(Token, nameof(Token))}");
        request.AddParameter("application/octet-stream", AssertNotNull(PublicKey, nameof(PublicKey)), ParameterType.RequestBody);

        return Execute<LoadKeys>(request);
      }
    }
  }
}




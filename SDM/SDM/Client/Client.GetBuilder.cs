using RestSharp;
using System.Numerics;
using System.Text;

namespace SDM {

  public partial class Client {
    public class GetBuilder : AbstractBuilder {

      public string? Token { get; internal set; }

      internal GetBuilder(string url) : base(url) {
      }

      public GetBuilder WithSession(Session value) {
        Token = value.Token;
        return this;
      }

      public object Execute(string resource) {

        var request = new RestRequest {
          Method = Method.Get,
          Resource = $"/{resource}"
        };

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", $"Bearer {AssertNotNull(Token, nameof(Token))}");

        return Execute<object>(request);
      }
    }
  }
}




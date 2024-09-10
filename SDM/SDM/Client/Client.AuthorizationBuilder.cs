using RestSharp;
using System.Runtime.ConstrainedExecution;
using static SDM.Client;

namespace SDM {

  public partial class Client {

    public class AuthorizationBuilder : AbstractBuilder {
      public string? Username { get; internal set; }
      public string? Password { get; internal set; }

      internal AuthorizationBuilder(string url) : base(url) {
      }

      public AuthorizationBuilder WithUsername(string value) {
        Username = value;
        return this;
      }

      public AuthorizationBuilder WithPassword(string value) {
        Password = value;
        return this;
      }


      public Session Execute() {
        var request = new RestRequest {
          Method = Method.Post,
          Resource = "/authorize",
          RequestFormat = DataFormat.Json
        };
        request.AddHeader("Accept", "application/json");
        request.AddJsonBody(Request.Authorize.Create
          .WithUserName(AssertNotNull(Username, nameof(Username)))
          .WithPassword(AssertNotNull(Password, nameof(Password))));

        var response = Execute<Response.Authorize>(request);

        return new Session() {
          Token = response.token
        };
      }
    }
  }
}




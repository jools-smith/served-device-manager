using RestSharp;
using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Transactions;
using static SDM.Client;
using static SDM.Response;

namespace SDM {

  public partial class Client {
    public string Url(string serverId) => $"https://{Producer}.compliance.flexnetoperations.{Domain}/api/1.0/instances/{serverId}";

    public string Producer { get; }
    public string Domain { get; }

    public Client() {
      Producer = Domain = string.Empty;
    }

    public Client(string producer, string domain) {
      Producer = producer;
      Domain = domain;
    }

    public PreviewBuilder CreatePreviewBuilder(string serverId) => new(Url(serverId));

    public AccessBuilder CreateAccessBuilder(string serverId) => new(Url(serverId));

    public AuthorizationBuilder CreateAuthorizationBuilder(string serverId) => new(Url(serverId));

    public LoadPublicKeyBuilder CreateLoadPublicKeyBuilder(string serverId) => new(Url(serverId));

    public GetBuilder CreateGetBuilder(string serverId) => new(Url(serverId));

    public Host CreateHost(string type, string value) => new(type, value);

    public Token CreateToken(string jwt) => new(jwt);
  }
}

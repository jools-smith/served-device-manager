using RestSharp;
using static SDM.Response;

namespace SDM {

  public partial class Client {
    public class PreviewBuilder : AbstractBuilder {
      public List<Request.Feature> Features { get; } = new();
      public String? Jwt { get; internal set; }
      public Host? Device { get; internal set; }

      internal PreviewBuilder(string url) : base(url) {
      }


      public PreviewBuilder WithToken(Token value) {
        Jwt = value.Jwt;
        return this;
      }

      public PreviewBuilder WithHost(Host value) {
        Device = value;
        return this;
      }

      public PreviewBuilder WithFeature(string name, string version = "0", long count = 1) => WithFeature(new Request.Feature() {
        name = name,
        version = version,
        count = count,
      });

      public PreviewBuilder WithFeature(Request.Feature value) {
        Features.Add(value);
        return this;
      }

      public PreviewBuilder WithFeature(IEnumerable<Request.Feature> value) {
        Features.AddRange(value);
        return this;
      }

      public Preview? Execute() {
        var request = new RestRequest {
          Method = Method.Post,
          Resource = "/preview_request",
          RequestFormat = DataFormat.Json
        };
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", $"Bearer {AssertNotNull(Jwt, nameof(Jwt))}");
        request.AddJsonBody(Request.Preview.Create
          .WithHost(base.AssertNotNull(Device, nameof(Device)))
          .WithFeatures(this.Features));

        return base.Execute<Preview>(request);
      }
    }
  }
}

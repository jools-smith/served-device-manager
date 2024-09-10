using RestSharp;

namespace SDM {

  public partial class Client {
    public class AccessBuilder : AbstractBuilder {

      /** properties **/
      public List<Request.Feature> Features { get; } = new();
      public Boolean Incremental { get; internal set; }
      public Boolean Partial { get; internal set; }
      public String? Jwt { get; internal set; }
      public String? BorrowInterval { get; internal set; }
      public Host? Device { get; internal set; }


      internal AccessBuilder(string url) : base(url) {
      }


      public AccessBuilder WithToken(Token value) {
        Jwt = value.Jwt;
        return this;
      }

      public AccessBuilder WithHost(Host value) {
        Device = value;
        return this;
      }

      public AccessBuilder WithIncremental(bool value) {
        Incremental = value;
        return this;
      }

      public AccessBuilder WithPartial(bool value) {
        Partial = value;
        return this;
      }

      public AccessBuilder WithBorrowInterval(string value) {
        BorrowInterval = value;
        return this;
      }

      public AccessBuilder WithFeature(string name, string version = "0", long count = 1) => WithFeature(new Request.Feature() {
        name = name,
        version = version,
        count = count,
      });

      public AccessBuilder WithFeature(Request.Feature value) {
        Features.Add(value);
        return this;
      }

      public AccessBuilder WithFeature(IEnumerable<Request.Feature> value) {
        Features.AddRange(value);
        return this;
      }

      public object Execute() {

        var request = new RestRequest {
          Method = Method.Post,
          Resource = "/access_request",
          RequestFormat = DataFormat.Json
        };
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", $"Bearer {AssertNotNull(Jwt, nameof(Jwt))}");
        request.AddJsonBody(Request.Access.Create
          .WithHost(AssertNotNull(Device, nameof(Device)))
          .WithIncremental(Incremental)
          .WithPartial(Partial)
          .WithBorrowInterval(AssertNotNull(BorrowInterval, nameof(BorrowInterval)))
          .WithIncremental(Incremental)
          .WithFeatures(Features));

        return base.Execute<Response.Access>(request);
      }
    }
  }
}




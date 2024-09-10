using Org.BouncyCastle.Bcpg.Sig;
using System.Text.Json.Serialization;
using static SDM.Client;

namespace SDM {
  public partial class Request {

    public class Access {
      public Host? hostId { get; set; }

      [JsonPropertyName("borrow-interval")]
      public string? borrowInterval { get; set; }
      public bool partial { get; set; }
      public bool incremental { get; set; }
      public List<Feature> features { get; set; } = new ();
      public Dictionary<string, string> selectorsDictionary { get; set; } = new();
      public Dictionary<string, string> vendorDictionary { get; set; } = new();
      public static Access Create => new();

      public Access WithHost(Host value) {
        this.hostId = value;
        return this;
      }

      public Access WithFeature(string name, string version, long count) => WithFeature(new Feature() {
        name = name,
        version = version,
        count = count
      });

      public Access WithFeatures(IEnumerable<Feature>features) {
        this.features.AddRange(features);
        return this;
      }

      public Access WithPartial(bool value) {
        this.partial = value;
        return this;
      }
      public Access WithIncremental(bool value) {
        this.incremental = value;
        return this;
      }

      public Access WithBorrowInterval(string value) {
        this.borrowInterval = value;
        return this;
      }

      public Access WithFeature(Feature feature) {
        this.features.Add(feature);
        return this;
      }

      public Access WithSelector(string key, string value) {
        this.selectorsDictionary[key] = value;
        return this;
      }

      public Access WithDictionaryEntry(string key, string value) {
        this.vendorDictionary[key] = value;
        return this;
      }
    }
  }
}

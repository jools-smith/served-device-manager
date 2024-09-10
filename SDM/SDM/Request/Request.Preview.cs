using System.Text.Json.Serialization;
using static SDM.Client;

namespace SDM {
  public partial class Request {
    public class Preview {

      public Host? hostId { get; set; }
      public List<Feature> features { get; set; } = new();
      public Dictionary<string, string> selectorsDictionary { get; set; } = new();
      public static Preview Create => new();

      public Preview WithHost(Host value) {
        this.hostId = value;
        return this;
      }

      public Preview WithFeature(string name, string version, long count) => WithFeature(new Feature() {
        name = name,
        version = version,
        count = count
      });

      public Preview WithFeatures(IEnumerable<Feature> features) {
        this.features.AddRange(features);
        return this;
      }

      public Preview WithFeature(Feature feature) {
        this.features.Add(feature);
        return this;
      }

      public Preview WithSelector(string key, string value) {
        this.selectorsDictionary[key] = value;
        return this;
      }
    }
  }
}


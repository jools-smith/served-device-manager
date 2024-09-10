using RestSharp;
using System.Text.Json;

namespace SDM {

  public partial class Client {
    public abstract class AbstractBuilder {
      public string Url { get; private set; }

      protected AbstractBuilder(string url) {
        this.Url = url;
      }

      protected T Execute<T>(RestRequest request) {
        var client = new RestClient(this.Url);

        var task = client.ExecuteAsync(request);
        task.Wait();

        if (task.Result.StatusCode != System.Net.HttpStatusCode.OK) {
          throw new InvalidDataException(task.Result.StatusCode.ToString());
        }

        if (task.Result.Content == null) {
          throw new ArgumentNullException(nameof(task.Result.Content));
        }

        var response = JsonSerializer.Deserialize<T>(task.Result.Content);

        if (response == null) {
          throw new ArgumentNullException(nameof(response));
        }

        return response;
      }

      protected T AssertNotNull<T>(T? value, string? name) => value != null ? value : throw new NullReferenceException(name);
    }
  }
}

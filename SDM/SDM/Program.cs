using Org.BouncyCastle.Bcpg.Sig;
using SDM;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text.Json;
using static SDM.Client;

internal class Program {

  static void dump(object? obj) {

    if (obj != null) {
      Console.Clear();
      Console.WriteLine(obj.GetType().Name);

      Console.WriteLine(JsonSerializer.Serialize(obj, new JsonSerializerOptions() {
        WriteIndented = true
      }));

      Console.WriteLine();
    }
  }

  class Config {
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Producer { get; set; } = "";
    public string Domain { get; set; } = "";
    public string Server { get; set; } = "";
  }

  static Config config = new Config() {
    Username = "producer",
    Password = "Flex4All!",
    Producer = "flex1113-uat",
    Domain = "com",
    Server = "HY9N3PSRMXZJ"
  };

  private static void Main(string[] args) {

    Console.WriteLine("Hello, World!");
    try {

      var client = new Client(producer: config.Producer, domain: config.Domain);
      dump(client);

      var host = client.CreateHost(type: "user", value: "dave@blokes.com");
      dump(host);


      var session = client.CreateAuthorizationBuilder(config.Server)
        .WithUsername(config.Username)
        .WithPassword(config.Password)
        .Execute();
      dump(session);

      var get = client.CreateGetBuilder(config.Server)
        .WithSession(session);

      dump(get.Execute("features"));
      dump(get.Execute("features/summaries"));
      dump(get.Execute("clients"));
      dump(get.Execute("configuration"));

      var factory = JwtFactory.Generate(2048);

      var jwt = client.CreateToken(factory.Jwt(TimeSpan.FromHours(1)));
      dump(jwt);

      var loadKeysResponse = client.CreateLoadPublicKeyBuilder(config.Server)
        .WithSession(session)
        .WithPublicKeyDer(factory.PublicKeyDer)
        .Execute();
      dump(loadKeysResponse);

      var previewResponse = client.CreatePreviewBuilder(config.Server)
         .WithToken(jwt)
         .WithHost(host)
         //.WithFeature("flexflex.feature.a", "0", 1L)
         //.WithFeature("flexflex.feature.b", "0", 1L)
         //.WithFeature("flexflex.feature.c", "0", 1L)
         //.WithFeature("flexflex.feature.x", "0", 1L)
         //.WithFeature("flexflex.feature.y", "0", 1L)
         //.WithFeature("flexflex.feature.z", "0", 1L)
         .Execute();
      dump(previewResponse);

      var accessResponse = client.CreateAccessBuilder(config.Server)
          .WithToken(jwt)
          .WithHost(host)
          .WithPartial(false)
          .WithIncremental(false)
          .WithBorrowInterval("0")
          .WithFeature("flexflex.feature.a", "0", 1L)
          .WithFeature("flexflex.feature.b", "0", 1L)
          .WithFeature("flexflex.feature.c", "0", 1L)
          .WithFeature("flexflex.feature.x", "0", 1L)
          .WithFeature("flexflex.feature.y", "0", 1L)
          .WithFeature("flexflex.feature.z", "0", 1L)
          .Execute();
      dump(accessResponse);

      accessResponse = client.CreateAccessBuilder(config.Server)
        .WithToken(jwt)
        .WithHost(host)
        .WithIncremental(true)
        .WithBorrowInterval("0")
        .Execute();
      dump(accessResponse);

    }
    catch (Exception e) {
      Console.WriteLine(e);
    }
    finally {
      Console.WriteLine("bye...");
    }
  }

}

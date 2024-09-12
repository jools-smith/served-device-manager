using Jose;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using SDM;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static SDM.Client;
using Config = SDM.Config;

namespace SDM {

  //class Optional<T> {
  //  private T? reference;

  //  public T Value {
  //    get => this.reference ?? throw new NullReferenceException(typeof(T).FullName);
  //    set => this.reference = value;
  //  }

  //  public T Set<V>(Func<V, T> action, V v) => reference = action.Invoke(v);
  //  public V If<V>(Func<T, V> action) => action.Invoke(this.Value);
  //}

  public class MyAbb {
    public static JsonSerializerOptions options = new JsonSerializerOptions() {
      WriteIndented = true
    };

    static Request.Feature permission = new Request.Feature() {
      name = "myabb.advanced",
      version = "2024.09",
      count = 1,
    };

    public JwtFactory Factory { get; set; } = new();
    public Config Config { get; set; } = new();

    private Client Client { get; set; } = new();
    private Session Session { get; set; } = new();
    private Token Token { get; set; } = new();

    private static Func<string?, string?, bool> compare_text = (request, response) => response?.Equals(response) ?? false;

    private static Func<Request.Feature, Response.Feature, bool> compare_feature = (request, response) =>
      compare_text(request.name, response.name) &&
      compare_text(request.version, response.version);

    public MyAbb CreateClient() {

      Client = new Client(producer: Config.Producer, domain: Config.Domain);

      return this;
    }

    public MyAbb CreateSession() {

      Session = Client.CreateAuthorizationBuilder(Config.Server)
        .WithUsername(Config.Username)
        .WithPassword(Config.Password)
        .Execute();

      return this;
    }

    public MyAbb CreateToken() {

      Token = Client.CreateToken(Factory.Jwt(TimeSpan.FromHours(1)));

      return this;
    }

    public MyAbb LoadPublicKey() {
      Client.CreateLoadPublicKeyBuilder(Config.Server)
        .WithSession(Session)
        .WithPublicKeyDer(Factory.PublicKeyDer)
        .Execute();

      return this;
    }

    public bool UnregisterUser(string userId) {

      var host = Client.CreateHost(type: "user", value: userId);

      var response = Client.CreateAccessBuilder(Config.Server)
          .WithToken(Token)
          .WithHost(host)
          .WithPartial(false)
          .WithIncremental(false)
          .WithBorrowInterval("0")
          .Execute();

      return response.features == null;
    }

    public bool RegisterUser(string userId) {

      var host = Client.CreateHost(type: "user", value: userId);

      var response = Client.CreateAccessBuilder(Config.Server)
          .WithToken(Token)
          .WithHost(host)
          .WithPartial(false)
          .WithIncremental(false)
          .WithBorrowInterval("0")
          .WithFeature(permission)
          .Execute();

      return response.features?.Exists(feature => compare_feature.Invoke(permission, feature)) ?? false;
    }

    public bool ValidateUser(string userId) {

      var host = Client.CreateHost(type: "user", value: userId);

      var response = Client.CreateAccessBuilder(Config.Server)
          .WithToken(Token)
          .WithHost(host)
          .WithPartial(false)
          .WithIncremental(true)
          .WithBorrowInterval("0")
          .Execute();

      return response.features?.Exists(feature => compare_feature.Invoke(permission, feature)) ?? false;
    }

    public void Write(string path) {
      File.WriteAllText(path, JsonSerializer.Serialize(this, new JsonSerializerOptions() {
        WriteIndented = true
      }));
    }

    public static MyAbb Create(string producer, string domain, string username, string password, string server) {
      return new MyAbb() {
        Config = new Config() {
          Producer = producer,
          Domain = domain,
          Username = username,
          Password = password,
          Server = server
        },
        Factory = JwtFactory.Generate(2048)
      };
    }

    public static MyAbb Load(string path) {
      using (Stream stream = File.OpenRead(path)) {
        return JsonSerializer.Deserialize<MyAbb>(stream) ?? throw new NullReferenceException();
      }
    }
  }
}

internal class Program {
  public static Func<string?, string> to_string = txt => txt ?? throw new ArgumentNullException(nameof(txt));

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

  private void Test(Config config) {
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

  private static void Main(string[] args) {

    Console.WriteLine("Hello, World!");
    try {
      var config = new Config() {
        Username = "producer",
        Password = "Flex4all!",
        Producer = "flex20018-uat",
        Domain = "com",
        Server = "1EPF4R1TAPTD"
      };

      //var program = new Program();

      //program.Test(config);

      var filename = $"..\\..\\..\\..\\..\\servers\\{config.Server}.json";

      Directory.CreateDirectory(to_string.Invoke(Path.GetDirectoryName(Path.GetFullPath(filename))));

      if (!File.Exists(filename)) {

        MyAbb.Create(config.Producer, config.Domain, config.Username, config.Password, config.Server)
           .CreateClient()
           .CreateSession()
           .LoadPublicKey()
           .Write(filename);
      }
      else {
        var abb = MyAbb.Load(filename)
          .CreateClient()
          .CreateSession()
          .CreateToken();

        dump(abb);

        var userids = Enumerable.Range(1, 5)
            .Select(x => Guid.NewGuid().ToString().ToUpper().Replace("-", ""))
            .ToList();

        //userids.ForEach(x => {
        //  Console.WriteLine($"register {x} -> {abb.RegisterUser(x)}");
        //});

        userids.ForEach(x => {
          Console.WriteLine($"validate {x} -> {abb.ValidateUser(x)}");
        });
      }
    }
    catch (Exception e) {
      Console.WriteLine(e);
    }
    finally {
      Console.WriteLine("bye...");
    }
  }

}

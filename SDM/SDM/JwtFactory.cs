using Jose;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SDM {
  public class JwtFactory {
    private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    public string PrivateKeyPem { get; }
    public byte[] PublicKeyDer { get; }


    private JwtFactory(string pem, byte[] der) {
      PrivateKeyPem = pem;
      PublicKeyDer = der;
    }
    
    public string Jwt(JwsAlgorithm algorithm, Dictionary<string, object> claims) {

      using (var reader = new PemReader(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(PrivateKeyPem))))) {

        var keyPair = reader.ReadObject() as AsymmetricCipherKeyPair;

        var rsaParams = DotNetUtilities.ToRSAParameters(keyPair?.Private as RsaPrivateCrtKeyParameters);

        using (var rsa = new RSACryptoServiceProvider()) {
          rsa.ImportParameters(rsaParams);

          return JWT.Encode(claims, rsa, algorithm);
        }
      }
    }

    public string Jwt(TimeSpan duration) {
     
      var now = DateTime.Now.Subtract(epoch);

      var expiry = now.Add(duration);

      return Jwt(JwsAlgorithm.RS256, new Dictionary<string, object> {
          ["sub"] = "Authorization",
          ["iat"] = Convert.ToInt64(now.TotalSeconds),
          ["exp"] = Convert.ToInt64(expiry.TotalSeconds),
          ["roles"] = "ROLE_CAPABILITY"
        });
    }

    public static JwtFactory Generate(int keyLength) {
      var rsa = new RsaKeyPairGenerator();

      var parameters = new KeyGenerationParameters(new SecureRandom(), keyLength);

      rsa.Init(parameters);

      var keyPair = rsa.GenerateKeyPair();

      using (var stringWriter = new StringWriter()) {
        var writer = new PemWriter(stringWriter);

        writer.WriteObject(keyPair.Private);
        writer.Writer.Flush();

        //Export ASN.1 DER-encoded
        var info = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keyPair.Public);

        return new JwtFactory(stringWriter.ToString(), info.GetEncoded());
      }
    }
  }

}

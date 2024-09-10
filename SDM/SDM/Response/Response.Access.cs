using static SDM.Client;

namespace SDM {
  public partial class Response {
    public class Access {
      public Host? requestHostId { get; set; }
      public List<Feature>? features { get; set; }
      public long serverLicenseRenewInterval { get; set; }
      public List<Status>? statusList { get; set; }
    }
  }
}

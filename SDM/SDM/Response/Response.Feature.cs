namespace SDM {
  public partial class Response {
    public class Feature {
      public string? name { get; set; }
      public string? version { get; set; }
      public object? count { get; set; }
      public string? expires { get; set; }
      public string? finalExpiry { get; set; }
      public string? vendorString { get; set; }
      public string? serialNumber { get; set; }
      public string? issuer { get; set; }
      public string? notice { get; set; }
      public object? maxCount { get; set; }
      public long? renewInterval { get; set; }

      public class LineItemInfo {
        public string? productName { get; set; }
        public string? activationId { get; set; }
        public object? entitlementId { get; set; }
        public string? productVersion { get; set; }
      }
      public List<LineItemInfo>? lineItemsInfo { get; set; }
    }
  }
}

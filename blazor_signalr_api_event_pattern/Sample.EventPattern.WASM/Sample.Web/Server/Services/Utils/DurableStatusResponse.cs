namespace Sample.Web.Server.Services.Utils
{
    public class DurableStatusResponse
    {
        public string instanceId { get; set; }
        public string runtimeStatus { get; set; }
        public string input { get; set; }
        public string customStatus { get; set; }
        public string[] output { get; set; }
        public string createdTime { get; set; }
        public string lastUpdatedTime { get; set; }
    }
}

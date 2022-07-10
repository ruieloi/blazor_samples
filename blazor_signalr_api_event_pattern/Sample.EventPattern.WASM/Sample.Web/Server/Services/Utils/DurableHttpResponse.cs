namespace Sample.Web.Server.Services.Utils
{
    public class DurableHttpResponse
    {
        public string id { get; set; }
        public string StatusQueryGetUri { get; set; }
        public string sendEventPostUri { get; set; }
        public string terminatePostUri { get; set; }
        public string purgeHistoryDeleteUri { get; set; }
        public string restartPostUri { get; set; }

    }
}

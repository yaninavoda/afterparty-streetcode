using Newtonsoft.Json;

namespace Streetcode.BLL.Entities.Payment
{
    public class InvoiceInfo
    {
        [JsonConstructor]
        public InvoiceInfo(string invoiceId, string pageUrl)
        {
            InvoiceId = invoiceId;
            PageUrl = pageUrl;
        }

        [JsonProperty("invoiceId")]
        public string InvoiceId { get; }

        [JsonProperty("pageUrl")]
        public string PageUrl { get; }
    }
}

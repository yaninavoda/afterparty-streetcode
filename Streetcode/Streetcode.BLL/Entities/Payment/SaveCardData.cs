using Newtonsoft.Json;

namespace Streetcode.BLL.Entities.Payment
{
    public class SaveCardData
    {
        [JsonProperty("saveCard")]
        public bool SaveCard { get; set; }

        [JsonProperty("walletId")]
        public string WalletId { get; set; }
    }
}
using Newtonsoft.Json;

namespace TestApplication.Entity
{
    public class PaymentModel
    {
        [JsonProperty("account_id")]
        public string AccountId { get; set; }
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
    }
}

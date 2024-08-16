using Newtonsoft.Json;

namespace TestApplication.Entity
{
    public class BalanceModel
    {
        [JsonProperty("account_id")]
        public string AccountId { get; set; }
        public int Period { get; set; } // формат YYYYMM
        public decimal Calculation { get; set; }
        //public decimal InBalance {  get; set; }
        //  по тз лучше не использовать
    }
}

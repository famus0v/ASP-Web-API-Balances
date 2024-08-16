namespace TestApplication.Entity
{
    public class SummaryModel
    {
        public string PeriodName { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal Charged { get; set; }
        public decimal Paid { get; set; }
        public decimal ExitBalance { get; set; }
    }
}

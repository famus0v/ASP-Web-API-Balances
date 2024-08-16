using System.Globalization;
using TestApplication.Entity;
using TestApplication.Entity.Enum;
using TestApplication.Services.Interfaces;
using TestApplication.Tuls;

namespace TestApplication.Services.Implementations
{
    public class BalanceService(string balanceFilePath, string paymentFilePath) : IBalanceService
    {
        private readonly List<BalanceModel> _balanceEntries = JsonReader.LoadJsonFile<BalanceModel>(balanceFilePath);
        private readonly List<PaymentModel> _paymentEntries = JsonReader.LoadJsonFile<PaymentModel>(paymentFilePath);

        public decimal GetCurrentDebt(string accountId)
        {
            var balances = GetAllBalances(accountId);
            var payments = GetAllPayments(accountId);

            var summaryPayments = payments.Sum(x => x.Sum);
            var summaryBalances = balances.Sum(x => x.Calculation);

            return summaryBalances - summaryPayments;
        }

        public IEnumerable<SummaryModel> GetBalances(string accountId, PeriodType periodType)
        {
            var balances = GetAllBalances(accountId);
            var payments = GetAllPayments(accountId);

            var periodSet = new HashSet<string>();
            periodSet.UnionWith(balances.Select(b => GetPeriodName(b.Period.ToString(), periodType)));
            periodSet.UnionWith(payments.Select(p => GetPeriodName(p.Date.ToString("yyyyMM"), periodType)));

            var periods = periodSet.OrderBy(p => p).ToList();

            var groupedBalances = balances.GroupBy(b => GetPeriodName(b.Period.ToString(), periodType))
                .Select(g => new
                {
                    Period = g.Key,
                    Charged = g.Sum(b => b.Calculation),
                })
                .ToDictionary(x => x.Period, x => x.Charged);

            var groupedPayments = payments.GroupBy(p => GetPeriodName(p.Date.ToString("yyyyMM"), periodType))
                .Select(g => new
                {
                    Period = g.Key,
                    Paid = g.Sum(p => p.Sum),
                })
                .ToDictionary(x => x.Period, x => x.Paid);

            var result = new List<SummaryModel>();
            decimal previousClosingBalance = 0;

            foreach (var period in periods)
            {
                var paid = groupedPayments.TryGetValue(period, out decimal paymentValue) ? paymentValue : 0;
                var charged = groupedBalances.TryGetValue(period, out decimal balanceValue) ? balanceValue : 0;

                var openingBalance = previousClosingBalance;
                var closingBalance = openingBalance + charged - paid;

                var summary = new SummaryModel
                {
                    PeriodName = period,
                    OpeningBalance = openingBalance,
                    Charged = charged,
                    Paid = paid,
                    ExitBalance = closingBalance
                };

                result.Add(summary);
                previousClosingBalance = closingBalance;
            }

            return result.OrderByDescending(r => r.PeriodName);
        }

        private static string GetPeriodName(string period, PeriodType periodType)
        {
            var year = period.Substring(0, 4);
            var month = period.Substring(4, 2);

            switch (periodType)
            {
                case PeriodType.Year:
                    return year;
                case PeriodType.Quarter:
                    int quarter = (int.Parse(month) - 1) / 3 + 1;
                    return $"{year}Q{quarter}";
                case PeriodType.Month:
                default:
                    var date = DateTime.ParseExact(period, "yyyyMM", CultureInfo.InvariantCulture);
                    return date.ToString("yyyy-MM");
            }
        }
    
        private List<BalanceModel> GetAllBalances(string accountId) => _balanceEntries.Where(b => b.AccountId.ToString() == accountId).ToList();
        private List<PaymentModel> GetAllPayments(string accountId) => _paymentEntries.Where(b => b.AccountId.ToString() == accountId).ToList();
    }
}

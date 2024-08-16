using TestApplication.Entity;
using TestApplication.Entity.Enum;

namespace TestApplication.Services.Interfaces
{
    public interface IBalanceService
    {
        IEnumerable<SummaryModel> GetBalances(string accountId, PeriodType periodType);
        decimal GetCurrentDebt(string accountId);
    }
}

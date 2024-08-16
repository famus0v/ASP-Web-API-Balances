using Microsoft.AspNetCore.Mvc;
using TestApplication.Entity.Enum;
using TestApplication.Services.Interfaces;

namespace TestApplication.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BalancesController : ControllerBase
    {
        private readonly IBalanceService _balanceService;

        public BalancesController(IBalanceService balanceService) => _balanceService = balanceService;

        [HttpGet("{accountId}")]
        public IActionResult GetBalances(string accountId, [FromQuery] PeriodType periodType = PeriodType.Month)
        {
            var result = _balanceService.GetBalances(accountId, periodType).ToList();
            return Ok(result);
        }

        [HttpGet("currentDebt/{accountId}")]
        public IActionResult GetDebt(string accountId)
        {
            var result = _balanceService.GetCurrentDebt(accountId);
            return Ok(result);
        }
    }
}

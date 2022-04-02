using System;
using System.Linq;
using System.Threading.Tasks;
using DistillerieGtaRP.Enums;
using DistillerieGtaRP.FormsCustom;
using DistillerieGtaRP.Services.Stats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistillerieGtaRP.Controllers
{
    [Authorize(Roles = "Boss, CoBoss, Administration, Government")]
    [Route("stats")]
    public class StatsController : Controller
    {
        private readonly StatsService _statsService;

        public StatsController(StatsService statsService)
        {
            _statsService = statsService;
        }

        [Route("", Name = "stats.index")]
        public async Task<IActionResult> Index(StatsPeriodFormCustom statsPeriodFormCustom)
        {
            var stats = await _statsService.GetStatsAsync(statsPeriodFormCustom);
            var notBilledTransactions = await _statsService.BilledTransactionsAsync(statsPeriodFormCustom);
            var notBilledTransactionsPricesSum = notBilledTransactions.Sum(m => m.Price);
            
            var caSum = stats.Harvests.Where(m => m.Destination == Destination.Export).Sum(m => m.Quantity) * 2
                          + stats.StatsPricingList.Where(m => m.Company.Name == "Hen House").Sum(m => m.Price)
                          + stats.StatsPricingList.Where(m => m.Company.Name == "Yellow Jack").Sum(m => m.Price);

            var afterEmployeePayementSum = caSum - notBilledTransactionsPricesSum;

            var governmentSum = Math.Round(afterEmployeePayementSum * (decimal)0.1, 0);

            ViewBag.Stats = stats;
            ViewBag.CaSum = caSum;
            ViewBag.AfterEmployeePayementSum = afterEmployeePayementSum;
            ViewBag.GovernmentSum = governmentSum;
            
            return View(statsPeriodFormCustom);
        }
    }
}
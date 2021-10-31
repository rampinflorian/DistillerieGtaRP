using DistillerieManzibar.FormsCustom;
using DistillerieManzibar.Services.Stats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistillerieManzibar.Controllers
{
    [Authorize(Roles = "Boss, CoBoss")]
    [Route("stats")]
    public class StatsController : Controller
    {
        private readonly StatsService _statsService;

        public StatsController(StatsService statsService)
        {
            _statsService = statsService;
        }

        [Route("", Name = "stats.index")]
        public IActionResult Index(StatsPeriodFormCustom statsPeriodFormCustom)
        {
            ViewBag.Stats = _statsService.GetStats(statsPeriodFormCustom).Result;
            return View(statsPeriodFormCustom);
        }
    }
}
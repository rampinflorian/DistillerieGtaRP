using System.Threading.Tasks;
using DistillerieManzibar.FormsCustom;
using DistillerieManzibar.Services.Stats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Rotativa.AspNetCore;

namespace DistillerieManzibar.Controllers
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
            ViewBag.Stats = await _statsService.GetStats(statsPeriodFormCustom);

            return View(statsPeriodFormCustom);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistillerieManzibar.Data;
using DistillerieManzibar.FormsCustom;
using DistillerieManzibar.Models;
using DistillerieManzibar.Services.Stats;
using DistillerieManzibar.Services.Stats.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistillerieManzibar.Controllers
{
    [Authorize(Roles = "Boss, CoBoss")]
    [Route("stats")]
    public class StatsController : Controller
    {
        private readonly StatsService _statsService;
        private readonly ApplicationDbContext _context;

        public StatsController(StatsService statsService, ApplicationDbContext context)
        {
            _statsService = statsService;
            _context = context;
        }

        [Route("", Name = "stats.index")]
        public async Task<IActionResult> Index(StatsPeriodFormCustom statsPeriodFormCustom)
        {
            var stats = await _statsService.GetStats(statsPeriodFormCustom);

            var statsPricings = new List<StatsPricing>();

            foreach (var command in stats.Commands)
            {
                if (statsPricings.Exists(m =>
                    m.Company == command.Company && m.LiquidCategory == command.LiquidCategory))
                {
                    statsPricings.First(m =>
                            m.Company == command.Company && m.LiquidCategory == command.LiquidCategory).Price +=
                        command.Quantity * command.Pricing.Price;
                }
                else
                {
                    statsPricings.Add(new StatsPricing()
                    {
                        Company = command.Company,
                        Price = command.Quantity * command.Pricing.Price,
                        LiquidCategory = command.LiquidCategory
                    });
                }
            }
            
            ViewBag.Stats = stats;
            ViewBag.StatsPricings = statsPricings;
            
            
            return View(statsPeriodFormCustom);
        }
    }
}
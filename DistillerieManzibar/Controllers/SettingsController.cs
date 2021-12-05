using System;
using System.Linq;
using System.Threading.Tasks;
using DistillerieManzibar.Data;
using DistillerieManzibar.Forms;
using DistillerieManzibar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DistillerieManzibar.Controllers
{
    [Authorize(Roles = "Boss, CoBoss, Administration")]
    [Route("settings")]
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("", Name = "settings.index")]
        public async Task<IActionResult> Index()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name");
            ViewBag.Pricings = await _context.Pricings.OrderByDescending(m => m.CreatedAt).ToListAsync();


            return View(new SettingsForm()
            {
                Parameter = _context.Parameters.First(),
                Pricing = new Pricing()
            });
        }

        [Route("", Name = "settings.index.post")]
        [HttpPost]
        public async Task<IActionResult> Index([Bind("CompanyId, LiquidCategory, Price")] Pricing pricing)
        {
            ModelState.Remove("CreatedAt");
            if (!ModelState.IsValid)
            {
                return View(new SettingsForm()
                    {
                        Parameter = _context.Parameters.First(),
                        Pricing = pricing
                    }
                );
            }

            pricing.CreatedAt = DateTime.Now;

            await _context.AddAsync(pricing);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [Route("parameter", Name = "settings.parameter.post")]
        [HttpPost]
        public async Task<IActionResult> Parameter([Bind("parameterId, News")] Parameter parameter)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var parameterToUpdate = _context.Parameters.First();

            if (await TryUpdateModelAsync<Parameter>(
                parameterToUpdate, "Parameter",
                m => m.News
            ))
            {
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
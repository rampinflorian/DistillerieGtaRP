using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DistillerieManzibar.Data;
using DistillerieManzibar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DistillerieManzibar.Controllers
{
    [Route("commands")]
    [Authorize(Roles = "Learner, Boss, CoBoss, Leader, Employee, Administration")]
    public class CommandController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommandController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("", Name = "command.index")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Commands
                .Include(c => c.Company)
                .Include(m => m.ApplicationUsers)
                .Include(m => m.Pricing)
                .Include(m => m.CreatedBy)
                .Include(m => m.PayementRecipient)
                .OrderByDescending(m => m.CreatedAt);
            return View(await applicationDbContext.ToListAsync());
        }

        [Route("create", Name = "command.create")]
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name");
            return View();
        }

        [Route("create", Name = "command.create.post")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommandId,LiquidCategory,Quantity,CompanyId")] Command command)
        {
            ModelState.Remove("CreatedAt");
            ModelState.Remove("BilledAt");
            ModelState.Remove("DeliveryAt");
            if (ModelState.IsValid)
            {
                command.CreatedAt = DateTime.Now;
                command.Pricing = await _context.Pricings.OrderByDescending(m => m.CreatedAt).FirstAsync(m =>
                    m.CreatedAt <= command.CreatedAt && m.CompanyId == command.CompanyId &&
                    m.LiquidCategory == command.LiquidCategory);
                command.CreatedBy = await _userManager.GetUserAsync(User);
                _context.Add(command);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name");
            return View(command);
        }

        [Route("edit/{id:int}", Name = "command.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var command = await _context.Commands.FindAsync(id);
            if (command == null)
            {
                return NotFound();
            }

            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name", command.CompanyId);
            return View(command);
        }

        [Route("edit/{id:int}", Name = "command.edit.post")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("CommandId,LiquidCategory,DeliveryAt,BilletAt,CreatedAt,Quantity,CompanyId, PricingId")]
            Command command)
        {
            ModelState.Remove("CreatedAt");
            ModelState.Remove("BilledAt");
            ModelState.Remove("DeliveryAt");
            if (id != command.CommandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(command);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommandExists(command.CommandId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name", command.CompanyId);
            return View(command);
        }

        [Route("delete/{id:int}", Name = "command.delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var command = await _context.Commands
                .Include(c => c.Company)
                .FirstOrDefaultAsync(m => m.CommandId == id);
            if (command == null)
            {
                return NotFound();
            }

            return View(command);
        }

        [Route("delete/{id:int}", Name = "command.delete.post")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var command = await _context.Commands.FindAsync(id);
            _context.Commands.Remove(command);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommandExists(int id)
        {
            return _context.Commands.Any(e => e.CommandId == id);
        }

        [Route("billed/{id:int}", Name = "command.billed")]
        [Authorize(Roles = "Boss, CoBoss, Leader, Administration")]
        public async Task<IActionResult> Billed(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var command = await _context.Commands.FindAsync(id);
            command.BilledAt = DateTime.Now;
            command.PayementRecipient = await _userManager.GetUserAsync(User);
            _context.Update(command);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Route("delivery/{id:int}", Name = "command.delivery")]
        public async Task<IActionResult> Delivery(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var command = await _context.Commands.FindAsync(id);
            command.DeliveryAt = DateTime.Now;
            _context.Update(command);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Route("delivery-participation/{id:int}", Name = "command.delivery.participation")]
        public async Task<IActionResult> DeliveryParticipation(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var command = await _context.Commands.Include(m => m.ApplicationUsers)
                .FirstOrDefaultAsync(m => m.CommandId == id);
            var applicationUser = await _userManager.GetUserAsync(User);

            if (command.ApplicationUsers.All(m => m.Id != applicationUser.Id))
            {
                command.ApplicationUsers.Add(applicationUser);
                _context.Update(command);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [Route("delivery-pariticpation-remove/{id:int}", Name = "command.deliery.participation.remove")]
        public async Task<IActionResult> DeliveryParticipationRemove(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var command = await _context.Commands.Include(m => m.ApplicationUsers)
                .FirstOrDefaultAsync(m => m.CommandId == id);
            var applicationUser = await _userManager.GetUserAsync(User);

            if (command.ApplicationUsers.Any(m => m.Id == applicationUser.Id))
            {
                command.ApplicationUsers.Remove(applicationUser);
                _context.Update(command);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
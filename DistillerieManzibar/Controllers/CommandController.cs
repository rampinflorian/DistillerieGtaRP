using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DistillerieManzibar.Data;
using DistillerieManzibar.Models;
using Microsoft.AspNetCore.Authorization;

namespace DistillerieManzibar.Controllers
{
    [Route("commands")]
    public class CommandController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommandController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("", Name = "command.index")]
        [Authorize(Roles = "Boss, Employee")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Commands.Include(c => c.Company);
            return View(await applicationDbContext.ToListAsync());
        }

        [Route("create", Name = "command.create")]
        [Authorize(Roles = "Boss")]
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name");
            return View();
        }

        [Route("create", Name = "command.create.post")]
        [Authorize(Roles = "Boss")]
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
                _context.Add(command);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name", command.CompanyId);
            return View(command);
        }

        [Route("edit/{id:int}", Name = "command.edit")]
        [Authorize(Roles = "Boss")]
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
        [Authorize(Roles = "Boss")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommandId,LiquidCategory,CreatedAt,Quantity,CompanyId")] Command command)
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
        [Authorize(Roles = "Boss")]
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
        [Authorize(Roles = "Boss")]
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
    }
}

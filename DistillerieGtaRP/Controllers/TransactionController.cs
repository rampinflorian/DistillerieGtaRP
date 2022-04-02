using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistillerieGtaRP.Data;
using DistillerieGtaRP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DistillerieGtaRP.Controllers
{
    [Authorize(Roles = "Learner, Boss, CoBoss, Leader, Employee, Administration")]
    [Route("transactions")]
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("", Name = "transaction.index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Transaction
                .Include(m => m.ApplicationUser)
                .Where(m => m.PayementAt == null)
                .OrderByDescending(m => m.CreatedAt).ToListAsync());
        }

        [Route("create", Name = "transaction.create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("create", Name = "transaction.create.post")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("TransactionId,LiquidCategory,CreatedAt,ApplicationUserId,Destination,Quantity")]
            Transaction transaction)
        {
            ModelState.Remove("ApplicationUserId");
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                var applicationUser = await _userManager.GetUserAsync(User);

                transaction.CreatedAt = DateTime.Now;
                transaction.ApplicationUser = applicationUser;
                _context.Add(transaction);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(transaction);
        }

        [Route("edit/{id:int}", Name = "transaction.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _userManager.GetUserAsync(User);
            var transaction = await _context.Transaction.FindAsync(id);

            if (transaction == null || transaction.ApplicationUser != applicationUser)
            {
                return NotFound();
            }

            return View(transaction);
        }

        [Route("edit/{id:int}", Name = "transaction.edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("TransactionId,LiquidCategory,CreatedAt,ApplicationUserId,Destination,Quantity")]
            Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.TransactionId))
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

            return View(transaction);
        }

        [Route("delete/{id:int}", Name = "transaction.delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _userManager.GetUserAsync(User);
            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.TransactionId == id);

            if (transaction == null || transaction.ApplicationUser != applicationUser)
            {
                return NotFound();
            }

            return View(transaction);
        }

        [Route("delete/{id:int}", Name = "transaction.delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            _context.Transaction.Remove(transaction);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.TransactionId == id);
        }
    }
}
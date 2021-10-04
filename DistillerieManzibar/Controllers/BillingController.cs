using System;
using System.Linq;
using System.Threading.Tasks;
using DistillerieManzibar.Data;
using DistillerieManzibar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistillerieManzibar.Controllers
{
    [Authorize]
    [Route("billing")]
    public class BillingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BillingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("", Name = "billing.index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationUsers.OrderByDescending(m => m.Sold).ToListAsync());
        }

        [Route("payement/{applicationUserId}", Name = "billing.payement")]
        public async Task<IActionResult> Payment(string applicationUserId)
        {
            
            var currentApplicationUser = await _userManager.GetUserAsync(User);
            
            if (!await _userManager.IsInRoleAsync(currentApplicationUser, "Boss"))
            {
                return Unauthorized();
            }
            
            var applicationUser =await _context.ApplicationUsers.FirstOrDefaultAsync(m => m.Id == applicationUserId);
            if (applicationUser is null)
            {
                return NotFound();
            }

            applicationUser.Sold = 0;
            applicationUser.LastPayement = DateTime.Now;
            
            _context.Update(applicationUser);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using DistillerieManzibar.CustomQueryModel;
using DistillerieManzibar.Data;
using DistillerieManzibar.Data.Dapper;
using DistillerieManzibar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistillerieManzibar.Controllers
{
    [Authorize(Roles = "Boss, CoBoss, Leader, Employee")]
    [Route("billing")]
    public class BillingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CustomQuery _customQuery;

        public BillingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, Data.Dapper.CustomQuery customQuery)
        {
            _context = context;
            _userManager = userManager;
            _customQuery = customQuery;
        }

        [Route("", Name = "billing.index")]
        public async Task<IActionResult> Index()
        {
            var applicationUsers = await _context.ApplicationUsers.OrderByDescending(m => m.Sold).ToListAsync();
            
            var sqlTransaction = "SELECT ApplicationUserId, SUM(Quantity) as Quantity FROM [Transaction] WHERE PayementAt is null GROUP BY ApplicationUserId";
            var billedCustomQuery = await _customQuery.QueryAsync<BilledCustomQueryModel>(sqlTransaction);

            ViewBag.BilledCustomQuery = billedCustomQuery;
            return View(applicationUsers);
        }

        [Authorize(Roles = "Boss")]
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

            var parameters = new { applicationUserId = applicationUser.Id };
            
            var sqlTransaction = "UPDATE [Transaction] SET PayementAt = current_timestamp WHERE ApplicationUserId = @applicationUserId";
            var result = _customQuery.ExecuteAsync(sqlTransaction, new { applicationUserId = applicationUser.Id });
            

            return RedirectToAction(nameof(Index));
        }
    }
}
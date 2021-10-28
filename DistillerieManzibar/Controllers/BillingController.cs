using System;
using System.Linq;
using System.Threading.Tasks;
using DistillerieManzibar.CustomQueryModel;
using DistillerieManzibar.Data;
using DistillerieManzibar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistillerieManzibar.Controllers
{
    [Authorize(Roles = "Learner, Boss, CoBoss, Leader, Employee")]
    [Route("billing")]
    public class BillingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Data.Dapper.CustomQuery _customQuery;

        public BillingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, Data.Dapper.CustomQuery customQuery)
        {
            _context = context;
            _userManager = userManager;
            _customQuery = customQuery;
        }

        [Route("", Name = "billing.index")]
        public async Task<IActionResult> Index()
        {
            
            var sqlTransaction = "SELECT ANU.Id as ApplicationUserId, SUM(Quantity) as Quantity FROM [Transaction] LEFT JOIN AspNetUsers ANU on [Transaction].ApplicationUserId = ANU.Id WHERE PayementAt is null GROUP BY ApplicationUserId, ANU.Id";
            var notBilledTransaction = await _customQuery.QueryAsync<BilledCustomQueryModel>(sqlTransaction);
            ViewBag.NotBilledTransaction = notBilledTransaction;
            
            
            return View(await _context.ApplicationUsers.OrderByDescending(m => m.Percentage).ToListAsync());
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

            var sqlTransaction = "UPDATE [Transaction] SET PayementAt = current_timestamp WHERE ApplicationUserId = @applicationUserId";
            var parameters = new { applicationUserId };
            var transactionWeek = await _customQuery.ExecuteAsync(sqlTransaction, parameters);
            
            return RedirectToAction(nameof(Index));
        }
    }
}
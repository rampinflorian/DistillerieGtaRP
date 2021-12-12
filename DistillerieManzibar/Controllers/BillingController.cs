using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistillerieManzibar.CustomQueryModel;
using DistillerieManzibar.Data;
using DistillerieManzibar.Models;
using DistillerieManzibar.Services.Stats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistillerieManzibar.Controllers
{
    [Authorize(Roles = "Learner, Boss, CoBoss, Leader, Employee, Administration")]
    [Route("billing")]
    public class BillingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Data.Dapper.CustomQuery _customQuery;
        private readonly StatsService _statsService;

        public BillingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, Data.Dapper.CustomQuery customQuery, StatsService statsService)
        {
            _context = context;
            _userManager = userManager;
            _customQuery = customQuery;
            _statsService = statsService;
        }

        [Route("", Name = "billing.index")]
        public async Task<IActionResult> Index()
        {
            var applicationUsers = new List<ApplicationUser>();
            applicationUsers.AddRange(await _userManager.GetUsersInRoleAsync("Boss"));
            applicationUsers.AddRange(await _userManager.GetUsersInRoleAsync("CoBoss"));
            applicationUsers.AddRange(await _userManager.GetUsersInRoleAsync("Leader"));
            applicationUsers.AddRange(await _userManager.GetUsersInRoleAsync("Employee"));
            applicationUsers.AddRange(await _userManager.GetUsersInRoleAsync("Learner"));
            
            ViewBag.NotBilledTransaction = await _statsService.NotBilledTransactionsAsync();
            
            return View(applicationUsers);
        }

        [Authorize(Roles = "Boss, Administration")]
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

            var sqlTransaction = "UPDATE [Transaction] SET PayementAt = current_timestamp WHERE ApplicationUserId = @applicationUserId AND PayementAt IS NULL";
            var parameters = new { applicationUserId };
            var transactionWeek = await _customQuery.ExecuteAsync(sqlTransaction, parameters);
            
            return RedirectToAction(nameof(Index));
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using DistillerieGtaRP.Data;
using DistillerieGtaRP.Enums;
using DistillerieGtaRP.FormsCustom;
using DistillerieGtaRP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DistillerieGtaRP.Controllers
{
    [Authorize(Roles = "Boss, CoBoss, Administration")]
    [Route("employee")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Route("", Name = "users.index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.ApplicationUsers = await _context.ApplicationUsers.ToListAsync();

            return View();
        }

        [Route("{id}", Name = "users.edit")]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUsers.FindAsync(id);
            if (applicationUser is null)
            {
                return NotFound();
            }

            var role = await _userManager.GetRolesAsync(applicationUser);

            var applicationRole = ApplicationRole.Learner;
            if (role.Any())
            {
                applicationRole = (ApplicationRole)Enum.Parse(typeof(ApplicationRole), role.First());
            }

            var applicationUserFormCustom = new ApplicationUserFormCustom()
            {
                ApplicationUser = applicationUser,
                ApplicationRole = applicationRole
            };

            return View(applicationUserFormCustom);
        }

        [Route("{id}", Name = "users.edit.post")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUserFormCustom applicationUserFormCustom)
        {
            if (!ModelState.IsValid)
            {
                return View(applicationUserFormCustom);
            }

            var applicationUserToUpdate = _context.ApplicationUsers.FirstOrDefault(m => m.Id == id);

            if (await TryUpdateModelAsync<ApplicationUser>(applicationUserToUpdate, "ApplicationUser",
                m => m.Percentage
                ))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                                                 "Try again, and if the problem persists, " +
                                                 "see your system administrator.");
                }
            }

            var roles = await _userManager.GetRolesAsync(applicationUserToUpdate);
            await _userManager.RemoveFromRolesAsync(applicationUserToUpdate, roles);
            await _userManager.AddToRoleAsync(applicationUserToUpdate, applicationUserFormCustom.ApplicationRole.ToString());

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            return View(applicationUserFormCustom);
        }
    }
}
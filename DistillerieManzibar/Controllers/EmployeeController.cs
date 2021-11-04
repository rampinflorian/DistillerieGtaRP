using System.Linq;
using System.Threading.Tasks;
using DistillerieManzibar.Data;
using DistillerieManzibar.Enums;
using DistillerieManzibar.FormsCustom;
using DistillerieManzibar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DistillerieManzibar.Controllers
{
    [Authorize(Roles = "Boss, CoBoss")]
    [Route("employee")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
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

            var applicationUserFormCustom = new ApplicationUserFormCustom()
            {
                ApplicationUser = applicationUser,
                ApplicationRole = ApplicationRole.Employee
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
                return RedirectToAction(nameof(Index));
            }
            
            return View(applicationUserFormCustom);
        }
    }
}
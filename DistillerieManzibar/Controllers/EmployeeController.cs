using System.Threading.Tasks;
using DistillerieManzibar.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            return View(applicationUser);

        }
    }
}
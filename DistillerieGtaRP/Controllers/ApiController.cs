using System.Threading.Tasks;
using DistillerieGtaRP.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistillerieGtaRP.Controllers
{
    public class ApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Learner, Boss, CoBoss, Leader, Employee, Administration, Government")]
        [Route("API")]
        public async Task<IActionResult> GetCurrentCommandsCount()
        {
            return Ok(await _context.Commands.CountAsync(m => m.BilledAt == null && m.DeliveryAt == null));
        }
    }
}
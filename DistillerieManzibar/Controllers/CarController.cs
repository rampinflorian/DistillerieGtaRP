using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistillerieManzibar.Data;
using DistillerieManzibar.Enums;
using DistillerieManzibar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DistillerieManzibar.Controllers
{
    [Authorize(Roles = "Learner, Boss, CoBoss, Leader, Employee")]
    [Route("cars")]
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CarController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("", Name = "car.index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.CarLogs = await _context.CarLogs
                .Include(m => m.ApplicationUser)
                .Include(m => m.Car).Take(10)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
            
            return View(await _context.Car.Include(m => m.ApplicationUser).ToListAsync());
        }

        [Route("key/attribute/{id:int}", Name = "car.key.attribute")]
        public async Task<IActionResult> KeyAttribute(int id)
        {
            var car = await _context.Car.FirstOrDefaultAsync(m => m.CarId == id);

            if (car is null)
            {
                return NotFound();
            }

            var applicationUser = await _userManager.GetUserAsync(User);

            if (car.ApplicationUser == null)
            {
                car.ApplicationUser = applicationUser;
                _context.Update(car);

                var carLog = new CarLog()
                {
                    CarDirection = CarDirection.Get,
                    ApplicationUser = applicationUser,
                    CreatedAt = DateTime.Now,
                    Car = car
                };
                _context.Add(carLog);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [Route("key/removed/{id:int}", Name = "car.key.removed")]
        public async Task<IActionResult> KeyRemoved(int id)
        {
            var car = await _context.Car.Include(m => m.ApplicationUser).FirstOrDefaultAsync(m => m.CarId == id);
            var applicationUser = await _userManager.GetUserAsync(User);

            if (car is null || (car.ApplicationUser != applicationUser))
            {
                return NotFound();
            }

            car.ApplicationUser = null;
            _context.Update(car);

            var carLog = new CarLog()
            {
                CarDirection = CarDirection.Drop,
                ApplicationUser = applicationUser,
                CreatedAt = DateTime.Now,
                Car = car
            };
            _context.Add(carLog);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
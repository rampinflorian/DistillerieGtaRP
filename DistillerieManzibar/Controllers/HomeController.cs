using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DistillerieManzibar.CustomQuery;
using DistillerieManzibar.Data;
using DistillerieManzibar.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DistillerieManzibar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DistillerieManzibar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly Data.Dapper.CustomQuery _customQuery;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, Data.Dapper.CustomQuery customQuery)
        {
            _logger = logger;
            _context = context;
            _customQuery = customQuery;
        }
        [Authorize(Roles = "Boss, CoBoss, Leader, Employee")]
        [Route("", Name = "home.index")]
        public async Task<IActionResult> Index()
        {
            var cars = await _context.Car.Include(m => m.ApplicationUser).ToListAsync();
            
            var carsDictionary = new Dictionary<string, int>
            {
                { "Used", cars.Count(m => m.ApplicationUser != null) },
                { "Free", cars.Count(m => m.ApplicationUser is null) }
            };
            ViewBag.CarsDictionary = carsDictionary;

            var transactionHistoryExport = _context.Transaction.Where(m => m.Destination == Destination.Export && m.CreatedAt > DateTime.Now.AddDays(-7)).Sum(m => m.Quantity);
            var transactionHistoryStock = _context.Transaction.Where(m => m.Destination == Destination.Stock && m.CreatedAt > DateTime.Now.AddDays(-7)).Sum(m => m.Quantity);
            
            var transactionHistoryDictionary = new Dictionary<string, int>()
            {
                { "transactionHistoryExport", transactionHistoryExport},
                { "transactionHistoryStock", transactionHistoryStock }
            };
            ViewBag.TransactionDictionary = transactionHistoryDictionary;

            
            var sqlTransaction = "SELECT TOP (7) SUM(Quantity) as Quantity, CONVERT(DATE,CreatedAt) as CreatedAt FROM [Transaction] GROUP BY CONVERT(DATE,CreatedAt) ORDER BY CONVERT(DATE,CreatedAt) DESC";
            var transactionWeek = await _customQuery.QueryAsync<TransactionCustomQuery>(sqlTransaction);

            ViewBag.TransactionWeek = transactionWeek;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
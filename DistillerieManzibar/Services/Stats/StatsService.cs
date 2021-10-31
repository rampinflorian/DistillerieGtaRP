using System;
using System.Linq;
using System.Threading.Tasks;
using DistillerieManzibar.Data;
using DistillerieManzibar.Enums;
using DistillerieManzibar.FormsCustom;
using Microsoft.EntityFrameworkCore;

namespace DistillerieManzibar.Services.Stats
{
    public class StatsService
    {
        private readonly ApplicationDbContext _context;

        public StatsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Model.Stats> GetStats(StatsPeriodFormCustom statsPeriodFormCustom)
        {
            statsPeriodFormCustom.Start ??= DateTime.Now.AddDays(-7);
            statsPeriodFormCustom.Stop ??= DateTime.Now;

            var stats = new Model.Stats
            {
                SumApplicationUser = await _context.Transaction.GroupBy(m => m.ApplicationUserId).CountAsync(),
                Harvests = await _context.Transaction.Where(m =>
                        m.CreatedAt >= statsPeriodFormCustom.Start && m.CreatedAt <= statsPeriodFormCustom.Stop)
                    .ToListAsync(),
                Commands = await _context.Commands.Include(m => m.Company).Where(m =>
                        m.CreatedAt >= statsPeriodFormCustom.Start && m.CreatedAt <= statsPeriodFormCustom.Stop)
                    .ToListAsync()
            };

            return stats;
        }
    }
}
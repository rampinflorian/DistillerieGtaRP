using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistillerieManzibar.Data;
using DistillerieManzibar.Enums;
using DistillerieManzibar.FormsCustom;
using DistillerieManzibar.Services.Stats.Model;
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
            statsPeriodFormCustom.Start ??= DateTime.Now.Date.AddDays(-6);
            statsPeriodFormCustom.Stop ??= DateTime.Now.Date;

            statsPeriodFormCustom.Stop = statsPeriodFormCustom.Stop.Value.AddDays(+1).AddSeconds(-1);

            var stats = new Model.Stats
            {
                SumApplicationUser = await _context.Transaction.GroupBy(m => m.ApplicationUserId).CountAsync(),
                Harvests = await _context.Transaction.Where(m =>
                        m.CreatedAt >= statsPeriodFormCustom.Start && m.CreatedAt <= statsPeriodFormCustom.Stop)
                    .ToListAsync(),
                Commands = await _context.Commands.Include(m => m.Pricing).Include(m => m.Company).Where(m =>
                        m.CreatedAt >= statsPeriodFormCustom.Start && m.CreatedAt <= statsPeriodFormCustom.Stop)
                    .ToListAsync()
            };
            stats.StatsPricingList = _GetPricing(stats);
            
            return stats;
        }

        private List<StatsPricing> _GetPricing(Model.Stats stats)
        {
            var statsPricings = new List<StatsPricing>();

            foreach (var command in stats.Commands)
            {
                if (statsPricings.Exists(m =>
                    m.Company == command.Company && m.LiquidCategory == command.LiquidCategory))
                {
                    statsPricings.First(m =>
                            m.Company == command.Company && m.LiquidCategory == command.LiquidCategory).Price +=
                        command.Quantity * command.Pricing.Price;
                }
                else
                {
                    statsPricings.Add(new StatsPricing()
                    {
                        Company = command.Company,
                        Price = command.Quantity * command.Pricing.Price,
                        LiquidCategory = command.LiquidCategory
                    });
                }
            }

            return statsPricings;
        }
    }
}
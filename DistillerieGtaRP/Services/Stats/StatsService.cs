using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistillerieGtaRP.CustomQueryModel;
using DistillerieGtaRP.Data;
using DistillerieGtaRP.Enums;
using DistillerieGtaRP.FormsCustom;
using DistillerieGtaRP.Models;
using DistillerieGtaRP.Services.Stats.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DistillerieGtaRP.Services.Stats
{
    public class StatsService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Data.Dapper.CustomQuery _customQuery;

        public StatsService(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            Data.Dapper.CustomQuery customQuery)
        {
            _context = context;
            _userManager = userManager;
            _customQuery = customQuery;
        }

        public async Task<Model.Stats> GetStatsAsync(StatsPeriodFormCustom statsPeriodFormCustom)
        {
            var _statsPeriodFromCustom = _PrepareDate(statsPeriodFormCustom);


            var users = new List<ApplicationUser>();
            users.AddRange(await _userManager.GetUsersInRoleAsync("Employee"));
            users.AddRange(await _userManager.GetUsersInRoleAsync("Learner"));
            users.AddRange(await _userManager.GetUsersInRoleAsync("Leader"));
            users.AddRange(await _userManager.GetUsersInRoleAsync("Boss"));
            users.AddRange(await _userManager.GetUsersInRoleAsync("CoBoss"));

            var stats = new Model.Stats
            {
                SumApplicationUser = users.Count,
                Harvests = await _context.Transaction.Where(m =>
                        m.CreatedAt >= _statsPeriodFromCustom.Start && m.CreatedAt <= _statsPeriodFromCustom.Stop)
                    .ToListAsync(),
                Commands = await _context.Commands.Include(m => m.Pricing).Include(m => m.Company).Where(m =>
                        m.CreatedAt >= _statsPeriodFromCustom.Start && m.CreatedAt <= _statsPeriodFromCustom.Stop)
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

        public async Task<IEnumerable<BilledCustomQueryModel>> NotBilledTransactionsAsync()
        {
            var sqlTransaction =
                @"SELECT u.id as ApplicationUserId, SUM(t.quantity) as Quantity, TMP.Date as LastPayementAt, 0 as Price
            FROM [Transaction] t
                LEFT JOIN AspNetUsers u ON t.ApplicationUserId = u.id
            LEFT JOIN (SELECT MAX(previous.PayementAt) as Date, u.Id as UserId
            FROM AspNetUsers u
                LEFT JOIN [Transaction] previous
                ON previous.ApplicationUserId = u.id and previous.PayementAt is not null
            group by u.Id
                ) as TMP on UserId = ApplicationUserId
            WHERE t.PayementAt is null
            Group by u.id, tmp.Date";

            var notBilledTransactions = await _customQuery.QueryAsync<BilledCustomQueryModel>(sqlTransaction);
            var billedCustomQueryModels = notBilledTransactions.ToList();

            return await _GetStatsFromTransactionsAsync(billedCustomQueryModels);

        }

        public async Task<IEnumerable<BilledCustomQueryModel>> BilledTransactionsAsync(
            StatsPeriodFormCustom statsPeriodFormCustom)
        {
            var _statsPeriodFromCustom = _PrepareDate(statsPeriodFormCustom);
            
            var sqlTransaction =
                $@"SELECT u.id as ApplicationUserId, SUM(t.quantity) as Quantity, TMP.Date as LastPayementAt, 0 as Price
            FROM [Transaction] t
                LEFT JOIN AspNetUsers u ON t.ApplicationUserId = u.id
            LEFT JOIN (SELECT MAX(previous.PayementAt) as Date, u.Id as UserId
            FROM AspNetUsers u
                LEFT JOIN [Transaction] previous
                ON previous.ApplicationUserId = u.id and previous.PayementAt is not null
            group by u.Id
                ) as TMP on UserId = ApplicationUserId
            WHERE t.CreatedAt >= '{_statsPeriodFromCustom.Start.Value:yyyyMMdd}' AND t.CreatedAt <= '{_statsPeriodFromCustom.Stop.Value:yyyyMMdd}'
            Group by u.id, tmp.Date";
            
            var notBilledTransactions = await _customQuery.QueryAsync<BilledCustomQueryModel>(sqlTransaction);
            var billedCustomQueryModels = notBilledTransactions.ToList();

            return await _GetStatsFromTransactionsAsync(billedCustomQueryModels);
        }

        private async Task<IEnumerable<BilledCustomQueryModel>>_GetStatsFromTransactionsAsync(List<BilledCustomQueryModel> billedCustomQueryModels)
        {
            var applicationUsers = new List<ApplicationUser>();
            applicationUsers.AddRange(await _userManager.GetUsersInRoleAsync("Boss"));
            applicationUsers.AddRange(await _userManager.GetUsersInRoleAsync("CoBoss"));
            applicationUsers.AddRange(await _userManager.GetUsersInRoleAsync("Leader"));
            applicationUsers.AddRange(await _userManager.GetUsersInRoleAsync("Employee"));
            applicationUsers.AddRange(await _userManager.GetUsersInRoleAsync("Learner"));
            
            foreach (var notBilledTransaction in billedCustomQueryModels)
            {
                var percentage = applicationUsers.FirstOrDefault(m => m.Id == notBilledTransaction.ApplicationUserId)
                    ?.Percentage ?? 0;
                notBilledTransaction.Price =
                    Math.Round(notBilledTransaction.Quantity * 2 * ((decimal)percentage / 100), 0);
            }

            return billedCustomQueryModels;
        }

        private StatsPeriodFormCustom _PrepareDate(StatsPeriodFormCustom statsPeriodFormCustom)
        {
            statsPeriodFormCustom.Start ??= DateTime.Now.Date.AddDays(-6);
            statsPeriodFormCustom.Stop ??= DateTime.Now.Date;

            statsPeriodFormCustom.Stop = statsPeriodFormCustom.Stop.Value.AddDays(+1).AddSeconds(-1);

            return statsPeriodFormCustom;
        }
    }
}
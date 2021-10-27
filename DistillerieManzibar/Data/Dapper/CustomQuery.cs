using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace DistillerieManzibar.Data.Dapper
{
    public class CustomQuery
    {
        private readonly ApplicationDbContext _context;
        private DbConnection _connection;
        public CustomQuery(ApplicationDbContext context)
        {
            _context = context;
            _connection = _context.Database.GetDbConnection();
            _context.Database.OpenConnectionAsync();
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query)
        {
            return await _connection.QueryAsync<T>(query);
            
        }

        public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await _connection.ExecuteAsync(sql, param, transaction).ConfigureAwait(true);
        }
        
        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await _connection.QuerySingleAsync<T>(sql, param, transaction);
        }
    }
}
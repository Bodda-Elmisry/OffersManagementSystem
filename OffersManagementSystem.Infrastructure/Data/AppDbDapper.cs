using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using OffersManagementSystem.Application.IData;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace OffersManagementSystem.Infrastructure.Data
{
    public class AppDbDapper<T> : IAppDbDapper<T> where T : class
    {
        private readonly string _connectionString;

        public AppDbDapper(string ConnectionString)
        {
            _connectionString = ConnectionString;
        }

        public async Task<int> ExecuteAsync(string sql, object param = null)
        {
            var result = 0;

            using(var connection = new SqlConnection(_connectionString))
            {
                result =  await connection.ExecuteAsync(sql, param);
            }

            return result;
        }

        public async Task<DbDataReader> ExecuteReaderAsync(string sql, object param = null)
        {
            DbDataReader result;

            using (var connection = new SqlConnection(_connectionString))
            {
                result = await connection.ExecuteReaderAsync(sql, param);
            }

            return result;
        }

        public async Task<T> ExecuteScalarAsync(string sql, object param = null)
        {
            T result;

            using (var connection = new SqlConnection(_connectionString))
            {
                result = await connection.ExecuteScalarAsync<T>(sql, param);
            }

            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync(string sql, object param = null)
        {
            IEnumerable<T> result;

            using (var connection = new SqlConnection(_connectionString))
            {
                result = await connection.QueryAsync<T>(sql, param);
            }

            return result;
        }

        public async Task<T> QueryFirstOrDefaultAsync(string sql, object param = null)
        {
            T result;

            using (var connection = new SqlConnection(_connectionString))
            {
                result = await connection.QueryFirstOrDefaultAsync<T>(sql, param);
            }

            return result;
        }


    }
}

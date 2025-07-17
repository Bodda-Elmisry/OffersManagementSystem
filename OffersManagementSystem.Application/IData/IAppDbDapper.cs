using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffersManagementSystem.Application.IData
{
    public interface IAppDbDapper<T> where T : class
    {
        Task<int> ExecuteAsync(string sql, object param = null);

        Task<DbDataReader> ExecuteReaderAsync(string sql, object param = null);

        Task<T> ExecuteScalarAsync(string sql, object param = null);

        Task<IEnumerable<T>> QueryAsync(string sql, object param = null);

        Task<T> QueryFirstOrDefaultAsync(string sql, object param = null);
    }
}

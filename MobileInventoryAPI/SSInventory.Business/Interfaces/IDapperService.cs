using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SSInventory.Business.Interfaces
{
    public interface IDapperService
    {
        DbConnection GetDbconnection();
        T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);
        List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);
        int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<int> ExecuteAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<T> InsertAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<T> UpdateAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    }
}

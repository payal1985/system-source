using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SSInventory.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Business.Services
{
    public class DapperService : IDapperService
    {
        private readonly IConfiguration _config;
        private string Connectionstring = "Default";

        public DapperService(IConfiguration config)
        {
            _config = config;
        }
        public void Dispose()
        {

        }

        public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            return db.Execute(sp, parms, commandType: commandType);
        }

        public async Task<int> ExecuteAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            return await Task.FromResult(Execute(sp, parms, commandType));
        }

        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        }

        public async Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            return await Task.FromResult(Get<T>(sp, parms, commandType));
        }

        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            return db.Query<T>(sp, parms, commandType: commandType).ToList();
        }

        public async Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            return await Task.FromResult(GetAll<T>(sp, parms, commandType));
        }

        public DbConnection GetDbconnection()
        {
            return new SqlConnection(_config.GetConnectionString(Connectionstring));
        }

        public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public async Task<T> InsertAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            return await Task.FromResult(Insert<T>(sp, parms, commandType));
        }

        public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public async Task<T> UpdateAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            return await Task.FromResult(Insert<T>(sp, parms, commandType));
        }
    }
}

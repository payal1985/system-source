using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchIntegrationAPI.Repositories
{
    public interface IConnectionUtility
    {
        bool CheckDbConnection(out string msg);
        void CloseConnection(IDbConnection connection);
        IDataAdapter CreateAdapter(IDbCommand command);
        IDbCommand CreateCommand(string commandText, CommandType commandType, IDbConnection connection);
        IDbConnection CreateConnection();
        IDbDataParameter CreateParameter(IDbCommand command);
        IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType);
        DataSet Insert(string commandText, CommandType commandType, IDbDataParameter[] parameters, out DataSet dataSet);
        void CallStoredProcedureReturnString(string commandText, IDbDataParameter[] parameters, out string valueStr);

        DataSet GetDataSet(string commandText, CommandType commandType, IDbDataParameter[] parameters = null);
        void Insert(string commandText, CommandType commandType, IDbDataParameter[] parameters);
    }
}

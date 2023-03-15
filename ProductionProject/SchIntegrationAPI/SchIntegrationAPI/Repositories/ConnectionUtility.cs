using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SchIntegrationAPI.Repositories
{
    public class ConnectionUtility : IConnectionUtility
    {
        private string _connectionString;

        public ConnectionUtility(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool CheckDbConnection(out string msg)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    msg = string.Empty;
                    return true;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
                //throw new Exception($"Error in DB connection test on CheckDBConnection Message: { ex.Message }\n{ ex.StackTrace}");
            }
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public void CloseConnection(IDbConnection connection)
        {
            var sqlConnection = (SqlConnection)connection;
            sqlConnection.Close();
            sqlConnection.Dispose();
        }

        public IDbCommand CreateCommand(string commandText, CommandType commandType, IDbConnection connection)
        {
            return new SqlCommand
            {
                CommandText = commandText,
                Connection = (SqlConnection)connection,
                CommandType = commandType
            };
        }

        public IDataAdapter CreateAdapter(IDbCommand command)
        {
            return new SqlDataAdapter((SqlCommand)command);
        }

        public IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType)
        {

            return new SqlParameter { ParameterName = name, Size = size, Value = value, DbType = dbType };
        }

        public IDbDataParameter CreateParameter(IDbCommand command)
        {
            SqlCommand SQLcommand = (SqlCommand)command;
            return SQLcommand.CreateParameter();
        }

        public DataSet Insert(string commandText, CommandType commandType, IDbDataParameter[] parameters, out DataSet dataSet)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();

                using (var command = CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    var dataset = new DataSet();
                    var dataAdaper = CreateAdapter(command);
                    dataAdaper.Fill(dataset);

                    return dataSet = dataset;
                }
            }
        }

        public void CallStoredProcedureReturnString(string commandText, IDbDataParameter[] parameters, out string valueStr)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();

                using (var command = CreateCommand(commandText, CommandType.StoredProcedure, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    var value = command.ExecuteScalar();
                    valueStr = value.ToString();
                }
            }
        }

        public DataSet GetDataSet(string commandText, CommandType commandType, IDbDataParameter[] parameters = null)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();

                using (var command = CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    var dataset = new DataSet();
                    var dataAdaper = CreateAdapter(command);
                    dataAdaper.Fill(dataset);

                    return dataset;
                }
            }
        }

        public void Insert(string commandText, CommandType commandType, IDbDataParameter[] parameters)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();

                using (var command = CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    try
                    {
                        var str = command.ExecuteNonQuery();


                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
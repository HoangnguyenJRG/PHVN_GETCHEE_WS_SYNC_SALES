using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PHVN_WS_CORE.SERVICES.DbAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _configuration;
        public SqlDataAccess(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<IEnumerable<T>> QueryAsync<T, U>(string command,
                                                             U parameters,
                                                             string connectionId = "Default",
                                                             CommandType commandType = CommandType.Text)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(connectionId));

            return await connection.QueryAsync<T>(command, parameters, commandType: commandType);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string command,
                                                        string connectionId = "Default",
                                                        CommandType commandType = CommandType.Text)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(connectionId));

            return await connection.QueryAsync<T>(command, commandType: commandType);
        }

        public async Task SaveAsync<T>(string command,
                                         T parameters,
                                         string connectionId = "Default",
                                         CommandType commandType = CommandType.Text)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(connectionId));

            int ret = await connection.ExecuteAsync(command, parameters, commandType: commandType);

        }
    }
}

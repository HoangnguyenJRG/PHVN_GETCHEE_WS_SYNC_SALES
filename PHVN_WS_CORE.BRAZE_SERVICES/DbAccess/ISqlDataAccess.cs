using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace PHVN_WS_CORE.SERVICES.DbAccess
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> QueryAsync<T, U>(string command, U parameters, string connectionId = "Default", CommandType commandType = CommandType.Text);
        Task<IEnumerable<T>> QueryAsync<T>(string command, string connectionId = "Default", CommandType commandType = CommandType.Text);
        Task SaveAsync<T>(string command, T parameters, string connectionId = "Default", CommandType commandType = CommandType.Text);
    }
}

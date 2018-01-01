using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace BLL.Interfaces
{
    public interface IActionLogger: IDisposable
    {
        void AddLog(ActionLog log);
        IEnumerable<ActionLog> GetActionLogsAsync();
        IEnumerable<ActionLog> GetActionLogsAsync(string filterValue);
        void Save();
    }
}
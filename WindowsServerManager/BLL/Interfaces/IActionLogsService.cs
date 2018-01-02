using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace BLL.Interfaces
{
    public interface IActionLogsService: IDisposable
    {
        IEnumerable<ActionLog> GetActionLogs();
        IEnumerable<ActionLog> GetActionLogs(string filterValue);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Common;
using DataAccessLayer.Entities;

namespace BLL.Interfaces
{
    public interface IActionLogsService: IDisposable
    {
        IEnumerable<ActionLog> GetActionLogs();
        IEnumerable<ActionLog> GetActionLogs(string filterValue);
        PagedResults<ActionLog> GetPagedActionLogs(int page, int pageSize);
    }
}
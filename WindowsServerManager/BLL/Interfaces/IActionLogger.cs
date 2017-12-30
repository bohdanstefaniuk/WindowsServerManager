using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace BLL.Interfaces
{
    public interface IActionLogger
    {
        void AddLog(ActionLog log);
        Task<IEnumerable<ActionLog>> GetActionLogsAsync();
        Task<IEnumerable<ActionLog>> GetActionLogsAsync(string filterValue);
        void Save();
    }
}
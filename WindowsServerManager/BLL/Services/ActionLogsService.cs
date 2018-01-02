using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BLL.Services
{
    public class ActionLogsService: IActionLogsService
    {
        private readonly UnitOfWork _unitOfWork;

        public ActionLogsService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ActionLog> GetActionLogs()
        {
            return _unitOfWork.ActionLogs.GetAll();
        }

        public IEnumerable<ActionLog> GetActionLogs(string filterValue)
        {
            return _unitOfWork.ActionLogs.GetAll();
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

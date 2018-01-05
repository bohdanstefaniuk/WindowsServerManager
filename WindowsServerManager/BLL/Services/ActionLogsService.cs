using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Common;
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

        public PagedResults<ActionLog> GetPagedActionLogs(int page, int pageSize)
        {
            var actionLogs = _unitOfWork.ActionLogs.GetAll().OrderByDescending(x => x.StartExecution).ToList();
            var skipAmount = pageSize * (page - 1);
            var totalNumberOfRecords = actionLogs.Count;
            var mod = totalNumberOfRecords % pageSize;
            var totalPageCount = totalNumberOfRecords / pageSize + (mod == 0 ? 0 : 1);

            var result = actionLogs.Skip(skipAmount).Take(pageSize);

            return new PagedResults<ActionLog>
            {
                Results = result,
                PageNumber = page,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
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

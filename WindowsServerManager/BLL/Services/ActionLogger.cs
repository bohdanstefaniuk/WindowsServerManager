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
    public class ActionLogger : IActionLogger
    {
        private const int _logsMaxCount = 10;
        private static readonly ActionLogger Instance = new ActionLogger();
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly List<ActionLog> _logs = new List<ActionLog>();
        public static ActionLogger GetInstance()
        {
            return Instance;
        }

        private ActionLogger() { }

        public void AddLog(ActionLog log)
        {
            lock (_logs)
            {
                _logs.Add(log);
                if (_logs.Count >= _logsMaxCount)
                {
                    SaveLogs();
                }
            }
        }
        public void Save()
        {
            lock (_logs)
            {
                if (_logs.Count > 0)
                {
                    SaveLogs();
                }
            }
        }

        public IEnumerable<ActionLog> GetActionLogsAsync()
        {
            return _unitOfWork.ActionLogs.GetAll();
        }

        public IEnumerable<ActionLog> GetActionLogsAsync(string filterValue)
        {
            return _unitOfWork.ActionLogs.GetAll();
        }

        private void SaveLogs()
        {
            foreach (var actionLog in _logs)
            {
                _unitOfWork.ActionLogs.Create(actionLog);
            }
            _unitOfWork.Save();
            _logs.Clear();
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

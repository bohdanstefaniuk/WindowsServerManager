using System;
using System.Collections.Generic;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BLL.Infrastructure
{
    public class ActionLogger
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

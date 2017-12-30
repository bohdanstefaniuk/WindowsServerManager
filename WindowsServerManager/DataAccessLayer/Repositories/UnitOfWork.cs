using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.EF;

namespace DataAccessLayer.Repositories
{
    public class UnitOfWork: IDisposable
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        private SettingsRepository _settingsRepository;
        private ActionLogsRepository _actionLogsRepository;

        public SettingsRepository Settings => _settingsRepository ?? (_settingsRepository = new SettingsRepository(_dbContext));
        public ActionLogsRepository ActionLogs => _actionLogsRepository ?? (_actionLogsRepository = new ActionLogsRepository(_dbContext));

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        private bool _disposed;

        public virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _dbContext.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

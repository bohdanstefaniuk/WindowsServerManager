using System;
using System.Threading.Tasks;
using BLL.Interfaces;
using DataAccessLayer.Repositories;
using MssqlManager;

namespace BLL.Services
{
    public class DbService: IDbService
    {
        private readonly DbManager _dbManager;

        public DbService()
        {
            var settingsManager = new SettingsService(new UnitOfWork());
            var mssqlSettings = settingsManager.GetSettingByCode("ApplicationDbServer");

            _dbManager = new DbManager(mssqlSettings.Value);
        }

        public async Task DropDatabase(string db)
        {
            _dbManager.ConfigureConnectionString(db);

            if (await _dbManager.GetDatabaseExists())
            {
                _dbManager.DropDatabase();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
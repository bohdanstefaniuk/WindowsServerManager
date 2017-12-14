using System;
using System.Threading.Tasks;
using BLL.Interfaces;
using MssqlManager;

namespace BLL.Services
{
    public class DbService: IDbService
    {
        private readonly DbManager _dbManager;

        public DbService()
        {
            //var settingsManager = new SettingsService(new UnitOfWork());
            //var mssqlSettings = settingsManager.GetSettingByCode("ApplicationDbServer");

            _dbManager = new DbManager(@"WORK-MS-02\MSSQL2016");
        }

        public async Task DropDatabase(string db)
        {
            _dbManager.ConfigureConnectionString(db);
            await _dbManager.DropDatabaseAsync();
        }
        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        
    }
}
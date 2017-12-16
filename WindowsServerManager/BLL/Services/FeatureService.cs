using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using DataAccessLayer.Repositories;
using MssqlManager;
using MssqlManager.Dto;

namespace BLL.Services
{
    public class FeatureService: IFeatureService
    {
        private readonly FeatureManager _featureManager;

        public FeatureService()
        {
            //var settingsManager = new SettingsService(new UnitOfWork());
            //var mssqlSettings = settingsManager.GetSettingByCode("ApplicationDbServer");

            _featureManager = new FeatureManager(@"WORK-MS-02\MSSQL2016");
        }

        public async Task<List<FeatureDto>> GetFeatures(string db)
        {
            _featureManager.ConfigureConnectionString(db);
            var features = await _featureManager.GetFeatures();
            return features.ToList();
        }

        public async Task UpdateFeatures(IEnumerable<FeatureDto> featuresToUpdate, string db)
        {
            _featureManager.ConfigureConnectionString(db);
            await _featureManager.SetFeaturesState(featuresToUpdate.ToList());
        }

        public async Task<bool> GetFeatureTableExist(string db)
        {
            _featureManager.ConfigureConnectionString(db);
            return await _featureManager.GetFeatureTableExist();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MssqlManager.Dto;

namespace BLL.Interfaces
{
    public interface IFeatureService: IDisposable
    {
        Task<List<FeatureDto>> GetFeatures(string db);
        Task UpdateFeatures(IEnumerable<FeatureDto> featuresToUpdate, string db, int redisDb);
        Task<bool> GetFeatureTableExist(string db);
    }
}
using System.Collections.Generic;
using MssqlManager.Dto;

namespace WebUI.FullFramework.Models
{
    public class FeaturesComponentUpdateModel
    {
        public string Db { get; set; }
        public int RedisDb { get; set; }
        public List<FeatureDto> Features { get; set; }
    }
}
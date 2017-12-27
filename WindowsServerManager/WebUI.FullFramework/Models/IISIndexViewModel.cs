using BLL.Enums;
using WebUI.FullFramework.Enums;

namespace WebUI.FullFramework.Models
{
    public class IISIndexViewModel
    {
        public string SiteName { get; set; }

        public string ApplicationPath { get; set; }

        public string ApplicationPoolName { get; set; }

        public string Database { get; set; }

        public string RedisDatabase { get; set; }

        public IISSiteType SiteType { get; set; }
        
        public IISViewActionType ActionViewType { get; set; }

        public bool IsFeatureTableExist { get; set; }

        public bool IsPoolStoppingOrStopped { get; set; }

        public bool IsPoolStartingOrStarted { get; set; }
    }
}
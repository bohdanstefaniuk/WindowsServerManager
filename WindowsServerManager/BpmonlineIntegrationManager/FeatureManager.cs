using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpmonlineIntegrationManager
{
    public class FeatureManager
    {
        private readonly string _url;

        public FeatureManager(string serviceUrl)
        {
            _url = serviceUrl;
        }
    }
}

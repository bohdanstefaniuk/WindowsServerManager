using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BpmonlineIntegrationManager
{
    public class ServiceHelper
    {
        private readonly string _url;

        public ServiceHelper(string serviceUrl)
        {
            _url = serviceUrl;
        }

        public void ConnectService()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://localhost/Build7112/")
        }
    }
}

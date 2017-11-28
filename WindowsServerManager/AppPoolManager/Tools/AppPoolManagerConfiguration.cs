using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AppPoolManager.Tools
{
    internal sealed class AppPoolManagerConfiguration
    {
        internal static string AppConfigFileName => ConfigurationManager.AppSettings["AppConfigFileName"];

        internal static string RedisConnectionStringKey => ConfigurationManager.AppSettings["RedisConnectionStringKey"];
    }
}

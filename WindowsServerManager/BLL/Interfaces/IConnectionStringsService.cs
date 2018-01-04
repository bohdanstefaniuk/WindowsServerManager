using System;
using System.Configuration;
using BLL.Enums;

namespace BLL.Interfaces
{
    public interface IConnectionStringsService: IDisposable
    {
        string GetRedisDb(string siteName, bool isSite);
        string GetMssqlDb(string siteName, bool isSite);
        string GetConfigurationFilePath(string siteName, IISSiteType siteType);
        ConnectionStringsSection GetSiteConnectionStrings(string siteName, bool isSite);
    }
}
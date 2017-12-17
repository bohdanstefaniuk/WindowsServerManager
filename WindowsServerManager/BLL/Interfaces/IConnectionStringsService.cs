using System;
using System.Configuration;

namespace BLL.Interfaces
{
    public interface IConnectionStringsService: IDisposable
    {
        string GetRedisDb(string siteName, bool isSite);
        string GetMssqlDb(string siteName, bool isSite);
        ConnectionStringsSection GetSiteConnectionStrings(string siteName, bool isSite);
    }
}
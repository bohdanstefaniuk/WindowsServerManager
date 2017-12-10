using System;
using System.Linq;
using AppPoolManager;
using BLL.Dto;
using BLL.Enums;

namespace BLL.Services
{
    public class SiteInformationService: IDisposable
    {
        private readonly SitesManager _siteManager;

        public SiteInformationService()
        {
            _siteManager = new SitesManager();
        }

        public SiteInformation GetInformationBySiteType(string path, IISSiteType siteType)
        {
            switch (siteType)
            {
                case IISSiteType.Site:
                    return GetSiteInformation(path);
                case IISSiteType.Application:
                    return GetApplicationInformation(path);
                default:
                    return null;
            }
        }

        private SiteInformation GetSiteInformation(string path)
        {
            var siteInfo = new SiteInformation();
            var site = _siteManager.GetSiteByName(path);
            var applicationRoot =
                site.Applications.SingleOrDefault(a => a.Path == "/");
            var virtualRoot =
                applicationRoot.VirtualDirectories.SingleOrDefault(v => v.Path == "/");

            siteInfo.Name = site.Name;
            siteInfo.ApplicationPoolName = applicationRoot.ApplicationPoolName;
            siteInfo.PhysicalPath = virtualRoot.PhysicalPath;

            foreach (var siteBinding in site.Bindings)
            {
                siteInfo.BindingInformation.Add(new BindingInfo
                {
                    BindingInformation = siteBinding.BindingInformation,
                    Host = siteBinding.Host,
                    Protocol = siteBinding.Protocol
                });
            }

            return siteInfo;
        }

        private SiteInformation GetApplicationInformation(string path)
        {
            var siteInfo = new SiteInformation();
            var app = _siteManager.GetApplicationByPath(path);
            var virtualRoot =
                app.VirtualDirectories.SingleOrDefault(v => v.Path == "/");

            siteInfo.Name = app.Path.Split('/').FirstOrDefault();
            siteInfo.ApplicationPoolName = app.ApplicationPoolName;
            siteInfo.PhysicalPath = virtualRoot.PhysicalPath;

            return siteInfo;
        }

        public void Dispose()
        {
            _siteManager?.Dispose();
        }
    }
}
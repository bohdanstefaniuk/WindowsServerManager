using AppPoolManager;
using System;
using System.Collections.Generic;
using System.Linq;
using AppPoolManager.Tools;
using BLL.Dto;
using BLL.Services;

namespace TrainingConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Here you can execute your code withot running full web UI (ASP.NET MVC or ASP.NET Core)
            var siteManager = new SitesManager();
            var site = siteManager.GetSiteByName("Default");
            var appWorkD = siteManager.GetApplicationByPath("WORK_D");

            Console.WriteLine(appWorkD.ToString());
            Console.WriteLine(appWorkD.ApplicationPoolName);

            var siteB = site.Bindings.FirstOrDefault();

            Console.WriteLine(siteB.BindingInformation);
            Console.WriteLine(siteB.Host);
            Console.WriteLine(siteB.Protocol);

            // Delay
            Console.Read();
        }
    }
}

using AppPoolManager;
using System;
using System.Collections.Generic;
using System.Linq;
using AppPoolManager.Tools;
using BLL.Dto;
using BLL.Services;
using MssqlManager;

namespace TrainingConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Here you can execute your code withot running full web UI (ASP.NET MVC or ASP.NET Core)
            var sqlManager = new FeatureManager(@"WORK-MS-02\MSSQL2016", "BPMonline7111_BStefaniuk_WORK_3_Build");
            var features = sqlManager.GetFeatures().Result;

            foreach (var feature in features)
            {
                Console.WriteLine($"Id: {feature.Id}, Code: {feature.Code}, State: {feature.State}");
            }

            // Delay
            Console.Read();
        }
    }
}

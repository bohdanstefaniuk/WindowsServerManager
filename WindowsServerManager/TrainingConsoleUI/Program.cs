using AppPoolManager;
using System;
using System.Collections.Generic;
using System.Linq;
using AppPoolManager.Tools;
using BLL.Dto;
using BLL.Services;
using MssqlManager;
using MssqlManager.Dto;

namespace TrainingConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Here you can execute your code withot running full web UI (ASP.NET MVC or ASP.NET Core)
            //var sqlManager = new FeatureManager(@"WORK-MS-02\MSSQL2016");
            //sqlManager.ConfigureConnectionString("BPMonline7111_BStefaniuk_WORK_3_Build");
            //var features = sqlManager.GetFeatures().Result;
            ////sqlManager.SetFeaturesState(features.ToList());

            //foreach (var feature in features)
            //{
            //    Console.WriteLine($"Id: {feature.Id}, Code: {feature.Code}, State: {feature.State}");
            //}

            //var updateFeatures = new List<FeatureDto>
            //{
            //    new FeatureDto
            //    {
            //        Id = new Guid("294429AC-D295-4D63-BBCB-C782FC139DFD"),
            //        State = false
            //    },
            //    new FeatureDto
            //    {
            //        Id = new Guid("BAFF6A85-154E-4348-B489-CD6DA190FE0B"),
            //        State = true
            //    }
            //};

            //sqlManager.SetFeaturesState(updateFeatures).GetAwaiter();

            var featureService = new FeatureService();
            var features = featureService.GetFeatures("BPMonline7111_BStefaniuk_WORK_3_Build").GetAwaiter().GetResult();
            foreach (var feature in features)
            {
                Console.WriteLine($"Id: {feature.Id}, Code: {feature.Code}, State: {feature.State}");
            }

            // Delay
            Console.Read();
        }
    }
}

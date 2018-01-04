using AppPoolManager;
using System;
using System.Collections.Generic;
using System.Linq;
using AppPoolManager.Tools;
using BLL.Dto;
using BLL.Enums;
using BLL.Services;
using FileSystemManager;
using MssqlManager;
using MssqlManager.Dto;
using DataAccessLayer.Entities;

namespace TrainingConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Here you can execute your code withot running full web UI (ASP.NET MVC or ASP.NET Core)

            var fileReader = new FileReader();
            Console.WriteLine(fileReader.ReadTextFileAsync(@"C:\Projects\_Core7112\TSBpm\Src\Lib\Terrasoft.WebApp.Loader\Web.config").Result);

            // Delay
            Console.Read();
        }
    }
}

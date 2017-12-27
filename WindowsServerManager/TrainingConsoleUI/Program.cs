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

namespace TrainingConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Here you can execute your code withot running full web UI (ASP.NET MVC or ASP.NET Core)

            Console.ReadKey();

            var dbService = new DbService();
            dbService.DropDatabase("TestDatabase_delete").GetAwaiter();

            // Delay
            Console.Read();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.EF;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAccessLayer.SeedData
{
    internal static class SettingsSeed
    {
        internal static void SeedSettings(ApplicationDbContext dbContext)
        {
            var settings = new List<Settings>
            {
                new Settings
                {
                    Id = Guid.NewGuid(),
                    Name = "Сервер на котором размещены БД для сайтов",
                    Code = "ApplicationDbServer",
                    Value = "TestServer"
                },

                new Settings
                {
                    Id = Guid.NewGuid(),
                    Name = "Название файлов (с расширением) конфигурации.",
                    Code = "ConfigurationFileName",
                    Value = "Web.config"
                },

                new Settings
                {
                    Id = Guid.NewGuid(),
                    Name = "Ключи по которым осуществлять поиск строки подключения для Redis.",
                    Code = "RedisConnectionKeys",
                    Value = "redis"
                }
            };

            dbContext.Settings.AddRange(settings);
            dbContext.SaveChanges();
        }
    }
}
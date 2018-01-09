using System;
using System.Collections.Generic;
using DataAccessLayer.EF;
using DataAccessLayer.Entities;

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
                },
                new Settings
                {
                    Id = Guid.NewGuid(),
                    Name = "Bpmonline sys. admin user name",
                    Code = "BpmonlineUserName",
                    Value = "Supervisor"
                },
                new Settings
                {
                    Id = Guid.NewGuid(),
                    Name = "Bpmonline sys. admin user name",
                    Code = "BpmonlineUserPassword",
                    Value = "Supervisor"
                },
                new Settings
                {
                    Id = Guid.NewGuid(),
                    Name = "Bpmonline features service name",
                    Code = "BpmonlineFeaturesServiceName",
                    Value = "FeatureService"
                }
            };

            dbContext.Settings.AddRange(settings);
            dbContext.SaveChanges();
        }
    }
}
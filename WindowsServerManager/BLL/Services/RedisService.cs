using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;

namespace BLL.Services
{
    public class RedisService: IRedisService
    {
        private readonly RedisManager.RedisManager _redisManager;

        public RedisService()
        {
            _redisManager = new RedisManager.RedisManager();
        }

        public async Task FlushDatabaseAsync(int db)
        {
            await _redisManager.FlushDatabaseAsync(db);
        }

        public async Task FlushAllDatabasesAsync()
        {
            await _redisManager.FlushAllDatabasesAsync();
        }

        public void Dispose()
        {
            _redisManager?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

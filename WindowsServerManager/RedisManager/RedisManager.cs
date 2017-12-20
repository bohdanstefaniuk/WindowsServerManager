using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RedisManager
{
    public class RedisManager : IDisposable
    {
        private readonly ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect("localhost,allowAdmin=true");

        public async Task FlushDatabaseAsync(int db)
        {
            var server = _redis.GetServer("localhost:6379");
            await server.FlushDatabaseAsync(db);
        }

        public async Task FlushAllDatabasesAsync()
        {
            var server = _redis.GetServer("localhost");
            await server.FlushAllDatabasesAsync();
        }

        public void Dispose()
        {
            _redis?.Dispose();
        }
    }
}

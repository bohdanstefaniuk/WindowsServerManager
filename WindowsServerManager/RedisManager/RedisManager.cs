using System;
using StackExchange.Redis;

namespace RedisManager
{
    public class RedisManager : IDisposable
    {
        ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect("localhost");

        public async void FlushDatabaseAsync(int db)
        {
            var server = _redis.GetServer("localhost");
            await server.FlushDatabaseAsync(db);
        }

        public async void FluashAllDatabasesAsync()
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

using System;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IRedisService: IDisposable
    {
        Task FlushAllDatabasesAsync();
        Task FlushDatabaseAsync(int db);
    }
}
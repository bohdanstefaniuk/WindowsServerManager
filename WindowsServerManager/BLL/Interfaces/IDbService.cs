using System;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IDbService: IDisposable
    {
        Task DropDatabase(string db);
    }
}
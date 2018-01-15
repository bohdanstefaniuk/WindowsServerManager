using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Dto;
using BLL.Enums;
using Microsoft.Web.Administration;

namespace BLL.Interfaces
{
    /// <inheritdoc />
    /// TODO - Check if we need many interfaces for different types of services 
    /// Maybe we extract common interface for all services
    public interface IApplicationPoolService: IDisposable
    {
        bool StartPoolByName(string name);
        bool StopPoolByName(string name);
        bool RecyclePoolByName(string name);
        bool IsPoolStartingOrStarted(string poolName);
        bool IsPoolStoppingOrStopped(string poolName);
        IEnumerable<Application> GetApplicationsByPool(string poolName);
        Task DeleteApplicationAsync(DeleteApplicationDto dto);
    }
}
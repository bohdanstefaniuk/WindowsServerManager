﻿using System;

namespace BLL.Interfaces
{
    /// <inheritdoc />
    /// TODO - Check if we need many interfaces for different types of services 
    /// Maybe we extract common interface for all services
    public interface IApplicationPoolService: IDisposable
    {
        bool StartPoolByName(string name);
        bool StopPoolByName(string name);
        bool IsPoolStartingOrStarted(string poolName);
        bool IsPoolStoppingOrStopped(string poolName);
    }
}
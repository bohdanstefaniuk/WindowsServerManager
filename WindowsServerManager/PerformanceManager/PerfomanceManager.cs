using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfomanceManager.Dto;

namespace PerfomanceManager
{
    public class PerfomanceManager : IDisposable
    {
        private PerformanceCounter _cpuCounter;
        private PerformanceCounter _ramCounter;
        private PerformanceCounter _diskCounter;

        public PerfomanceManager()
        {
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public PerformanceDto GetCounters()
        {
            return new PerformanceDto
            {
                CpuCounter = _cpuCounter.NextValue(),
                RamCounter = _ramCounter.NextValue()
            };
        }

        public void GetCountersTask()
        {

        }

        public void GetCategories()
        {
            var categories = PerformanceCounterCategory.GetCategories();
            Console.WriteLine("-- Performance Counter Categories");
            foreach (var performanceCounterCategory in categories)
            {
                Console.WriteLine($"\tCategory name: {performanceCounterCategory.CategoryName}, Machine name: {performanceCounterCategory.CategoryHelp}");
            }
        }

        public void Dispose()
        {
            _cpuCounter?.Dispose();
            _ramCounter?.Dispose();
        }
    }
}

using System.Collections.Generic;
using System.IO;
using PerfomanceManager.Dto;

namespace PerformanceManager
{
    public class DiskManager
    {
        public IEnumerable<DriveUsageDto> GetDiskUsage()
        {
            var drives = DriveInfo.GetDrives();
            var drivesDtos = new List<DriveUsageDto>();

            foreach (var driveInfo in drives)
            {
                var driveDto = new DriveUsageDto { Name = driveInfo.Name };
                if (driveInfo.IsReady)
                {
                    driveDto.VolumeLabel = driveInfo.VolumeLabel;
                    driveDto.TotalFreeSpace = driveInfo.TotalFreeSpace;
                    driveDto.TotalSize = driveInfo.TotalSize;
                }

                drivesDtos.Add(driveDto);
            }

            return drivesDtos;
        }
    }
}

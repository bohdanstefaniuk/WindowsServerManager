using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemManager
{
    public class DriveManager
    {
        public DriveInfo[] GetDrivesList()
        {
            return DriveInfo.GetDrives();
        }

        public DriveInfo GetDriveInfo(char driveLabel)
        {
            var drivePath = $"{driveLabel}:\\";
            var drive = DriveInfo.GetDrives().FirstOrDefault(x => x.Name == drivePath);
            return drive;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemManager
{
    public class FileCleaner
    {
        /// <summary>
        /// Get folder exists stauts
        /// </summary>
        /// <param name="path">Path to folder</param>
        /// <returns>True when folder exists</returns>
        private bool IsFolderExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Delete folder and all inner data
        /// </summary>
        /// <param name="path">Path to folder</param>
        /// <returns>True when folder deleted</returns>
        public bool DeleteFolder(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            directoryInfo.Delete(true);

            return !IsFolderExists(path);
        }
    }
}

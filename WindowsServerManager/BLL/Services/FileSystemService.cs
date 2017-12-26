using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystemManager;

namespace BLL.Services
{
    public class FileSystemService
    {
        private readonly FileCleaner _fileCleaner;

        public FileSystemService()
        {
            _fileCleaner = new FileCleaner();
        }

        public void DeleteFolder(string path)
        {
            _fileCleaner.DeleteFolder(path);
        }
    }
}

using System.Collections.Generic;

namespace AppPoolManager.Dto
{
    public class ApplicationPath
    {
        public List<string> PathElements { get; set; }
        public string FullPath { get; set; }
    }
}

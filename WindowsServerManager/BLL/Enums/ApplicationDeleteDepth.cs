using System;

namespace BLL.Enums
{
    [Flags]
    public enum ApplicationDeleteDepth
    {
        ApplicationOrSite = 1,
        FileSystem = 2,
        ApplicationPool = 3
    }
}
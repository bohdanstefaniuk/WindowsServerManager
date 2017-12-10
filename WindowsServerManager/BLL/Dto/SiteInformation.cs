using System.Collections.Generic;

namespace BLL.Dto
{
    public class SiteInformation
    {
        public SiteInformation()
        {
            BindingInformation = new List<BindingInfo>();
        }

        public string PhysicalPath { get; set; }

        public string Name { get; set; }

        public string ApplicationPoolName { get; set; }

        public List<BindingInfo> BindingInformation { get; set; }
    }

    public class BindingInfo
    {
        public string BindingInformation { get; set; }
        public string Protocol { get; set; }
        public string Host { get; set; }
    }
}
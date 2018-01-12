using BLL.Enums;

namespace BLL.Dto
{
    public class DeleteApplicationDto
    {
        public string Name { get; set; }
        public string SiteName { get; set; }
        public string Database { get; set; }
        public IISSiteType SiteType { get; set; }
        public DeleteApplicationStates DeleteStates { get; set; }

        public DeleteApplicationDto()
        {
            DeleteStates = new DeleteApplicationStates();
        }
    }

    public class DeleteApplicationStates
    {
        public bool NeedDeleteApplication { get; set; }
        public bool NeedDeleteApplicationPool { get; set; }
        public bool NeedDeleteFiles { get; set; }
        public bool NeedDeleteDatabase { get; set; }
        public bool NeedDeleteChildApplications { get; set; }
    }
}
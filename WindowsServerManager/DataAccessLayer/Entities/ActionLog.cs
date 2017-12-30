using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class ActionLog: Entity
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string ActionParams { get; set; }
        public string OriginalUrl { get; set; }
        public string UserHost { get; set; }
        public string UserAgent { get; set; }
        public string HttpMethod { get; set; }
        public string Form { get; set; }
        public string Query { get; set; }
        public string Referer { get; set; }
        public string Headers { get; set; }
        public string Cookies { get; set; }
        public string UserName { get; set; }
        public DateTime StartExecution { get; set; }
        public DateTime FinishExecution { get; set; }
        public double ExecutionTime { get; set; }
    }
}

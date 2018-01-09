using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpmonlineIntegrationManager.DTO
{
    public class FeatureStateInfo
    {
        public string Code { get; set; }
        public int State { get; set; }
        public Guid SysAdminUnitId { get; set; }
    }
}

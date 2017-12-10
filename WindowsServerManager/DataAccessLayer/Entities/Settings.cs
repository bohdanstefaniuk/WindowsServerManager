using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Settings: Entity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Value { get; set; }
    }
}

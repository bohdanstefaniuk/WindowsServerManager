using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.EF;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories
{
    public class ActionLogsRepository : IRepository<ActionLog>
    {
        private readonly ApplicationDbContext _dbContext;

        public ActionLogsRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }


        public IEnumerable<ActionLog> GetAll()
        {
            return _dbContext.ActionLogs.ToList();
        }

        public ActionLog Get(Guid id)
        {
            return _dbContext.ActionLogs.SingleOrDefault(x => x.Id == id);
        }

        public ActionLog GetByCode(string code)
        {
            return null;
        }

        public void Create(ActionLog item)
        {
            _dbContext.ActionLogs.Add(item);
        }

        public void Update(ActionLog item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(Guid id)
        {
            ActionLog actionLog = _dbContext.ActionLogs.Find(id);
            if (actionLog != null)
            {
                _dbContext.ActionLogs.Remove(actionLog);
            }
        }
    }
}

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
    public class SettingsRepository: IRepository<Settings>
    {
        private readonly ApplicationDbContext _dbContext;

        public SettingsRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public IEnumerable<Settings> GetAll()
        {
            return _dbContext.Settings;
        }

        public Settings Get(Guid id)
        {
            return _dbContext.Settings.SingleOrDefault(x => x.Id == id);
        }

        public Settings GetByCode(string code)
        {
            return _dbContext.Settings.SingleOrDefault(x => x.Code == code);
        }

        public void Create(Settings item)
        {
            _dbContext.Settings.Add(item);
        }

        public void Update(Settings item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(Guid id)
        {
            Settings setting = _dbContext.Settings.Find(id);
            if (setting != null)
            {
                _dbContext.Settings.Remove(setting);
            }
        }
    }
}

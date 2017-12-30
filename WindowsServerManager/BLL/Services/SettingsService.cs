using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BLL.Services
{
    public class SettingsService: ISettingsService
    {
        private readonly UnitOfWork _unitOfWork;

        public SettingsService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Settings> GetSettings()
        {
            return _unitOfWork.Settings.GetAll().Result.ToList();
        }

        public Settings GetSettingsById(Guid id)
        {
            return _unitOfWork.Settings.Get(id).Result;
        }

        public Settings GetSettingByCode(string code)
        {
            return _unitOfWork.Settings.GetByCode(code);
        }

        public void CreateSettings(Settings settings)
        {
            var allSettings = _unitOfWork.Settings.GetAll().Result;
            if (allSettings.Any(x => x.Code == settings.Code))
            {
                throw new Exception("Данная настройка уже существует");
            }

            _unitOfWork.Settings.Create(settings);
            _unitOfWork.Save();
        }

        public void UpdateSetting(Settings settings)
        {
            _unitOfWork.Settings.Update(settings);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

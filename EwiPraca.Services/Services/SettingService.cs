using EwiPraca.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EwiPraca.Model;
using EwiPraca.Data.Interfaces;

namespace EwiPraca.Services.Services
{
    public class SettingService : ISettingService
    {
        private readonly IRepository<Setting> _settingRepository;
        private readonly IRepository<UserSetting> _userSettingRepository;

        public SettingService(IRepository<Setting> settingRepository,
            IRepository<UserSetting> userSettingRepository)
        {
            _settingRepository = settingRepository;
            _userSettingRepository = userSettingRepository;
        }

        public IEnumerable<Setting> AllSettings()
        {
            return _settingRepository.All().ToList();
        }

        public IEnumerable<UserSetting> AllUserSetting(string userId)
        {
            return _userSettingRepository.Query(x => x.ApplicationUserID == userId).ToList();
        }

        public int CreateUserSetting(UserSetting entity)
        {
            _userSettingRepository.Insert(entity);

            return entity.Id;
        }

        public int CreateSetting(Setting entity)
        {
            _settingRepository.Insert(entity);

            return entity.Id;
        }

        public void DeleteUserSetting(UserSetting entity)
        {
            _userSettingRepository.Update(entity);
        }

        public void DeleteSetting(Setting entity)
        {
            _settingRepository.Delete(entity);
        }

        public Setting GetSettingById(int id)
        {
            return _settingRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public UserSetting GetUserSettingById(int id)
        {
            return _userSettingRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public void UpdateUserSetting(UserSetting entity)
        {
            _userSettingRepository.Update(entity);
        }

        public void UpdateSetting(Setting entity)
        {
            _settingRepository.Update(entity);
        }
    }
}

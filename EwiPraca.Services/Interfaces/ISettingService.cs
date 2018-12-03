using EwiPraca.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EwiPraca.Services.Interfaces
{
    public interface ISettingService
    {
        int CreateSetting(Setting entity);

        void UpdateSetting(Setting entity);

        void DeleteSetting(Setting entity);

        Setting GetSettingById(int id);

        IEnumerable<Setting> AllSettings();

        int CreateUserSetting(UserSetting entity);

        void UpdateUserSetting(UserSetting entity);

        void DeleteUserSetting(UserSetting entity);

        UserSetting GetUserSettingById(int id);

        IEnumerable<UserSetting> AllUserSetting(string userId);
    }
}

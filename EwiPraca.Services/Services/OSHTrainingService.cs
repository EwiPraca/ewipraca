using EwiPraca.Data.Interfaces;
using EwiPraca.Model;
using EwiPraca.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EwiPraca.Services.Services
{
    public class OSHTrainingService : IOSHTrainingService
    {
        private readonly IRepository<OSHTraining> _oshTrainingRepository;

        public OSHTrainingService(IRepository<OSHTraining> oshTrainingRepository)
        {
            _oshTrainingRepository = oshTrainingRepository;
        }

        public IEnumerable<OSHTraining> All()
        {
            return _oshTrainingRepository.Query(x => !x.IsDeleted).ToList();
        }

        public int Create(OSHTraining entity)
        {
            _oshTrainingRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(OSHTraining entity)
        {
            entity.IsDeleted = true;
            _oshTrainingRepository.Update(entity);
        }

        public OSHTraining GetById(int id)
        {
            return _oshTrainingRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public List<OSHTraining> GetOSHTrainingsToExpire(int defaultDaysBeforeExpiration)
        {
            var now = DateTime.Now;
            var trainings = _oshTrainingRepository.Query(x => !x.IsDeleted && !x.ReminderSent && x.NextCompletionDate != null).ToList();

            if (trainings.Count > 0)
            {
                var training = trainings.FirstOrDefault();

                var userSetting = training.Employee.UserCompany.ApplicationUser.UserSettings?.FirstOrDefault(x => x.Setting.SettingName == "MedicalResultNumberOfDaysBeforeWarning");

                if (userSetting != null)
                {
                    defaultDaysBeforeExpiration = Convert.ToInt32(userSetting.SettingValue);
                }

                return trainings.Where(x => x.NextCompletionDate.Value.AddDays(-defaultDaysBeforeExpiration) > now).ToList();
            }
            else
            {
                return new List<OSHTraining>();
            }
        }

        public void Update(OSHTraining entity)
        {
            _oshTrainingRepository.Update(entity);
        }
    }
}

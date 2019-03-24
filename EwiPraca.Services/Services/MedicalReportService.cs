using EwiPraca.Data.Interfaces;
using EwiPraca.Model;
using EwiPraca.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EwiPraca.Services.Services
{
    public class MedicalReportService : IMedicalReportService
    {
        private readonly IRepository<MedicalReport> _medicalReportsRepository;

        public MedicalReportService(IRepository<MedicalReport> medicalReportsRepository)
        {
            _medicalReportsRepository = medicalReportsRepository;
        }

        public IEnumerable<MedicalReport> All()
        {
            return _medicalReportsRepository.Query(x => !x.IsDeleted).ToList();
        }
        
        public int Create(MedicalReport entity)
        {
            _medicalReportsRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(MedicalReport entity)
        {
            entity.IsDeleted = true;
            _medicalReportsRepository.Update(entity);
        }

        public List<MedicalReport> GetByCompanyId(int companyId)
        {
            return _medicalReportsRepository.Query(x => x.Employee.UserCompanyId == companyId && !x.IsDeleted).ToList();
        }

        public MedicalReport GetById(int id)
        {
            return _medicalReportsRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public List<MedicalReport> GetMedicalReportsToExpire(int defaultDaysBeforeExpiration)
        {
            var now = DateTime.Now;
            var reports = _medicalReportsRepository.Query(x => !x.IsDeleted && !x.ReminderSent && x.NextCompletionDate != null).ToList();

            if(reports.Count > 0)
            {
                var report = reports.FirstOrDefault();

                var userSetting = report.Employee.UserCompany.ApplicationUser.UserSettings?.FirstOrDefault(x => x.Setting.SettingName == "MedicalResultNumberOfDaysBeforeWarning");

                if(userSetting != null)
                {
                    defaultDaysBeforeExpiration = Convert.ToInt32(userSetting.SettingValue);
                }

                return reports.Where(x => x.NextCompletionDate.Value.AddDays(-defaultDaysBeforeExpiration) > now).ToList();
            }
            else
            {
                return new List<MedicalReport>();
            }
        }

        public void Update(MedicalReport entity)
        {
            _medicalReportsRepository.Update(entity);
        }
    }
}

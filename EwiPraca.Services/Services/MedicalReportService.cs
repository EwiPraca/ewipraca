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

        public MedicalReport GetById(int id)
        {
            return _medicalReportsRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public List<MedicalReport> GetMedicalReportsToExpire(int daysBeforeExpiration)
        {
            var now = DateTime.Now;
            return _medicalReportsRepository.Query(x => !x.IsDeleted && !x.ReminderSent && x.NextCompletionDate != null && x.NextCompletionDate.Value.AddDays(-daysBeforeExpiration) > now).ToList();

        }

        public void Update(MedicalReport entity)
        {
            _medicalReportsRepository.Update(entity);
        }
    }
}

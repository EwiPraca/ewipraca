using System.Collections.Generic;
using EwiPraca.Model.EmployeeArea;
using EwiPraca.Services.Interfaces;
using EwiPraca.Data.Interfaces;
using System.Linq;
using System;

namespace EwiPraca.Services.Services
{
    public class CustomEventService : ICustomEventService
    {
        private readonly IRepository<CustomEvent> _customEventRepository;

        public CustomEventService(IRepository<CustomEvent> customEventRepository)
        {
            _customEventRepository = customEventRepository;
        }

        public IEnumerable<CustomEvent> All()
        {
            return _customEventRepository.All().ToList();
        }

        public int Create(CustomEvent entity)
        {
            _customEventRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(CustomEvent entity)
        {
            entity.IsDeleted = true;
            _customEventRepository.Update(entity);
        }

        public List<CustomEvent> GetByCompanyId(int companyId)
        {
            return _customEventRepository.Query(x => !x.IsDeleted && x.CompanyId == companyId).ToList();
        }

        public CustomEvent GetById(int id)
        {
            return _customEventRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public List<CustomEvent> GetCustomEventsToRemind()
        {
            DateTime now = DateTime.Now.Date;

            return _customEventRepository.Query(x => !x.IsDeleted && x.StartDate.Date.AddDays(-1) == now && x.Reminder).ToList();
        }

        public void Update(CustomEvent entity)
        {
            _customEventRepository.Update(entity);
        }
    }
}

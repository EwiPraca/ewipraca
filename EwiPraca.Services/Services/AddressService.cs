using EwiPraca.Data.Interfaces;
using EwiPraca.Model.UserArea;
using EwiPraca.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace EwiPraca.Services.Services
{
    public class AddressService : IAddressService
    {
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<AddressType> _addressTypeRepository;

        public AddressService(IRepository<Address> addressRepository,
            IRepository<AddressType> addressTypeRepository)
        {
            _addressRepository = addressRepository;
            _addressTypeRepository = addressTypeRepository;
        }

        public IEnumerable<Address> All()
        {
            return _addressRepository.All().ToList();
        }

        public int Create(Address entity)
        {
            _addressRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(Address entity)
        {
            _addressRepository.Delete(entity);
        }

        public AddressType GetAddressTypeByName(string addressTypeName)
        {
            return _addressTypeRepository.Query(x => x.AddressTypeName == addressTypeName).FirstOrDefault();
        }

        public List<AddressType> GetAddressTypes()
        {
            return _addressTypeRepository.All().ToList();
        }

        public Address GetById(int id)
        {
            return _addressRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public void Update(Address entity)
        {
            _addressRepository.Update(entity);
        }
    }
}
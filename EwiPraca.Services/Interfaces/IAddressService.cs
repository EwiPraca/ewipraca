using EwiPraca.Model.UserArea;
using System.Collections.Generic;

namespace EwiPraca.Services.Interfaces
{
    public interface IAddressService : IService<Address>
    {
        List<AddressType> GetAddressTypes();

        AddressType GetAddressTypeByName(string addressTypeName);
    }
}
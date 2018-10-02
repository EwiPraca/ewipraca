using EwiPraca.Model.UserArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EwiPraca.Services.Interfaces
{
    public interface IAddressService : IService<Address>
    {
        List<AddressType> GetAddressTypes();
        AddressType GetAddressTypeByName(string addressTypeName);
    }
}

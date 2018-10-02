using AutoMapper;
using EwiPraca.Data;
using EwiPraca.Model.UserArea;
using EwiPraca.Models;

namespace EwiPraca
{
    public static class EwiClassMapper
    {
        public static void Setup()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ApplicationUser, UserViewModel>();
                cfg.CreateMap<AddressViewModel, Address>();
                cfg.CreateMap<Address, AddressViewModel>();
                cfg.CreateMap<UserCompany, UserCompanyViewModel>().ForMember(d => d.UserCompanyAddress,
                 opt => opt.MapFrom(x => x.Address));
                cfg.CreateMap<UserCompanyViewModel, UserCompany>().ForMember(d => d.Address,
                 opt => opt.MapFrom(x => x.UserCompanyAddress));
                cfg.CreateMap<UserViewModel, ApplicationUser>();
            }
            );
        }
    }
}
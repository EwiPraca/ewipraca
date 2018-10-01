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
                cfg.CreateMap<UserCompanyAddressViewModel, Address>();
                cfg.CreateMap<Address, UserCompanyAddressViewModel>();
                cfg.CreateMap<UserCompany, UserCompanyViewModel>().ForMember(d => d.UserCompanyAddress,
                 opt => opt.MapFrom(x => x.UserCompanyAddress));
                cfg.CreateMap<UserCompanyViewModel, UserCompany>().ForMember(d => d.UserCompanyAddress,
                 opt => opt.MapFrom(x => x.UserCompanyAddress));
                cfg.CreateMap<UserViewModel, ApplicationUser>();
            }
            );
        }
    }
}
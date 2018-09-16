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
                cfg.CreateMap<UserCompany, UserCompanyViewModel>();
                cfg.CreateMap<UserCompanyViewModel, UserCompany>();
                cfg.CreateMap<UserViewModel, ApplicationUser>();
                cfg.CreateMap<UserCompanyAddressViewModel, UserCompanyAddress>();
                cfg.CreateMap<UserCompanyAddress, UserCompanyAddressViewModel>();
            }
            );
        }
    }
}
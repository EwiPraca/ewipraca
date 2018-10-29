using AutoMapper;
using EwiPraca.Data;
using EwiPraca.Importers.Importers;
using EwiPraca.Model;
using EwiPraca.Model.UserArea;
using EwiPraca.Models;
using System;
using System.Linq;

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
                cfg.CreateMap<EmployeeViewModel, Employee>();
                cfg.CreateMap<Employee, EmployeeViewModel>().ForMember(d => d.MedicalReports,
                 opt => opt.MapFrom(x => x.MedicalReports.Where(s => !s.IsDeleted).ToList()))
                 .ForMember(d => d.OSHTrainings,
                 opt => opt.MapFrom(x => x.OSHTrainings.Where(s => !s.IsDeleted).ToList()))
                 .ForMember(d => d.Contracts,
                 opt => opt.MapFrom(x => x.Contracts.Where(s => !s.IsDeleted).ToList())); 

                cfg.CreateMap<EmployeeImportRow, Employee>()
                .ForMember(d => d.BirthDate,
                 opt => opt.MapFrom(x => Convert.ToDateTime(x.BirthDate)))
                .ForPath(d => d.Address.City,
                 opt => opt.MapFrom(x => x.City))
                 .ForPath(d => d.Address.StreetName,
                 opt => opt.MapFrom(x => x.StreetName))
                 .ForPath(d => d.Address.StreetNumber,
                 opt => opt.MapFrom(x => x.StreetNumber))
                 .ForPath(d => d.Address.PlaceNumber,
                 opt => opt.MapFrom(x => x.PlaceNumber))
                 .ForPath(d => d.Address.ZIPCode,
                 opt => opt.MapFrom(x => x.ZIPCode));

                cfg.CreateMap<EmployeeImportRow, EmployeeViewModel>();

                cfg.CreateMap<Contract, ContractViewModel>();
                cfg.CreateMap<ContractViewModel, Contract>().ForMember(d => d.Employee,
                 opt => opt.MapFrom(x => x.Employee));

                cfg.CreateMap<MedicalReport, MedicalReportViewModel>();
                cfg.CreateMap<MedicalReportViewModel, MedicalReport>().ForMember(d => d.Employee,
                 opt => opt.MapFrom(x => x.Employee));

                cfg.CreateMap<OSHTraining, OSHTrainingViewModel>()
                .ForMember(d => d.IsValid,
                 opt => opt.Ignore());
                cfg.CreateMap<OSHTrainingViewModel, OSHTraining>().ForMember(d => d.Employee,
                 opt => opt.MapFrom(x => x.Employee));
            }
            );
        }
    }
}
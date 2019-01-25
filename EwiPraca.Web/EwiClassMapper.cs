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
                cfg.CreateMap<ApplicationUser, UserViewModel>().ForMember(d => d.IsActive,
                 opt => opt.MapFrom(x => x.IsActive)); ;
                cfg.CreateMap<AddressViewModel, Address>();
                cfg.CreateMap<Address, AddressViewModel>();
                cfg.CreateMap<UserCompany, UserCompanyViewModel>().ForMember(d => d.UserCompanyAddress,
                 opt => opt.MapFrom(x => x.Address));
                cfg.CreateMap<UserCompanyViewModel, UserCompany>().ForMember(d => d.Address,
                 opt => opt.MapFrom(x => x.UserCompanyAddress));
                cfg.CreateMap<UserViewModel, ApplicationUser>().ForMember(d => d.IsActive,
                 opt => opt.MapFrom(x => x.IsActive));
                cfg.CreateMap<EmployeeViewModel, Employee>();
                cfg.CreateMap<Employee, EmployeeViewModel>().ForMember(d => d.MedicalReports,
                 opt => opt.MapFrom(x => x.MedicalReports.Where(s => !s.IsDeleted).ToList()))
                 .ForMember(d => d.OSHTrainings,
                 opt => opt.MapFrom(x => x.OSHTrainings.Where(s => !s.IsDeleted).ToList()))
                 .ForMember(d => d.Contracts,
                 opt => opt.MapFrom(x => x.Contracts.Where(s => !s.IsDeleted).ToList()))
                 .ForMember(d => d.SickLeaves,
                 opt => opt.MapFrom(x => x.SickLeaves.Where(s => !s.IsDeleted).ToList()));

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

                cfg.CreateMap<PositionDictionary, PositionDictionaryViewModel>();
                cfg.CreateMap<PositionDictionaryViewModel, PositionDictionary>();
                cfg.CreateMap<PositionDictionaryValue, PositionDictionaryValueViewModel>();
                cfg.CreateMap<PositionDictionaryValueViewModel, PositionDictionaryValue>();

                cfg.CreateMap<JobPartDictionary, JobPartDictionaryViewModel>();
                cfg.CreateMap<JobPartDictionaryViewModel, JobPartDictionary>();
                cfg.CreateMap<JobPartDictionaryValue, JobPartDictionaryValueViewModel>();
                cfg.CreateMap<JobPartDictionaryValueViewModel, JobPartDictionaryValue>();
                cfg.CreateMap<SickLeave, SickLeaveViewModel>();
                cfg.CreateMap<SickLeaveViewModel, SickLeave>();

                cfg.CreateMap<EmployeeImportRow, EmployeeViewModel>();
                cfg.CreateMap<UserSetting, UserSettingViewModel>()
                .ForMember(d => d.SettingDescription,
                 opt => opt.MapFrom(x => x.Setting.SettingDescription))
                 .ForMember(d => d.SettingType,
                 opt => opt.MapFrom(x => x.Setting.SettingValueType))
                 .ForMember(d => d.SettingValue,
                 opt => opt.MapFrom(x => x.SettingValue));

                cfg.CreateMap<Contract, ContractViewModel>();
                cfg.CreateMap<ContractViewModel, Contract>().ForMember(d => d.Employee,
                 opt => opt.MapFrom(x => x.Employee))
                 .ForMember(d => d.JobPartDictionaryValue,
                 opt => opt.MapFrom(x => x.JobPartDictionaryValue));

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
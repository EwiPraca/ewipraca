using Ewipraca.ImportProcessors;
using EwiPraca.App_Start.Identity;
using EwiPraca.Data;
using EwiPraca.Data.Interfaces;
using EwiPraca.Importers;
using EwiPraca.Model;
using EwiPraca.Model.UserArea;
using EwiPraca.Services.Interfaces;
using EwiPraca.Services.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Mvc5;

namespace EwiPraca
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager(),
                new InjectionConstructor(new EwiPracaDbContext()));
            container.RegisterType<IRoleStore<IdentityRole, string>, RoleStore<IdentityRole>>(new HierarchicalLifetimeManager(),
                new InjectionConstructor(new EwiPracaDbContext()));
            container.RegisterType<IAuthenticationManager>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            var injection = new InjectionConstructor();

            container.RegisterType<IEwiPracaDbContext, EwiPracaDbContext>(new HierarchicalLifetimeManager(), injection);
            container.RegisterType<ApplicationUserManager>(new HierarchicalLifetimeManager());
            container.RegisterType<ApplicationRoleManager>(new HierarchicalLifetimeManager());
            container.RegisterType<ApplicationSignInManager>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<ApplicationUser>, Repository<ApplicationUser>>();
            container.RegisterType<IRepository<UserCompany>, Repository<UserCompany>>();
            container.RegisterType<IRepository<Address>, Repository<Address>>();
            container.RegisterType<IRepository<AddressType>, Repository<AddressType>>();
            container.RegisterType<IRepository<Employee>, Repository<Employee>>();
            container.RegisterType<IRepository<Contract>, Repository<Contract>>();
            container.RegisterType<IRepository<MedicalReport>, Repository<MedicalReport>>();
            container.RegisterType<IRepository<OSHTraining>, Repository<OSHTraining>>();
            container.RegisterType<IRepository<PositionDictionary>, Repository<PositionDictionary>>();
            container.RegisterType<IRepository<PositionDictionaryValue>, Repository<PositionDictionaryValue>>();
            container.RegisterType<IRepository<JobPartDictionary>, Repository<JobPartDictionary>>();
            container.RegisterType<IRepository<JobPartDictionaryValue>, Repository<JobPartDictionaryValue>>();

            container.RegisterType<IUserCompanyService, UserCompanyService>();
            container.RegisterType<IAddressService, AddressService>();
            container.RegisterType<IEmployeeService, EmployeeService>();
            container.RegisterType<IContractService, ContractService>();
            container.RegisterType<IMedicalReportService, MedicalReportService>();
            container.RegisterType<IOSHTrainingService, OSHTrainingService>();
            container.RegisterType<IPositionDictionaryService, PositionDictionaryService>();
            container.RegisterType<IJobPartDictionaryService, JobPartDictionaryService>();

            container.RegisterType<IEwiImporter, CompanyEmployeeImporter>();

            container.RegisterType<IImportEmployeeProcessor, ImportEmployeeProcessor>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
using EwiPraca.Data.Interfaces;
using EwiPraca.Model;
using EwiPraca.Model.UserArea;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EwiPraca.Data
{
    public class EwiPracaDbContext : IdentityDbContext<ApplicationUser>, IEwiPracaDbContext
    {
        public EwiPracaDbContext()
            : base("EwiPracaDbContext", throwIfV1Schema: false)
        {
        }

        public IDbSet<UserCompany> UserCompanies { get; set; }
        public IDbSet<AddressType> AddressTypes { get; set; }
        public IDbSet<Address> Adresses { get; set; }
        public IDbSet<Employee> Employees { get; set; }
        public IDbSet<Contract> Contracts { get; set; }
        public IDbSet<MedicalReport> MedicalReports { get; set; }
        public IDbSet<OSHTraining> OSHTraining { get; set; }

        public static EwiPracaDbContext Create()
        {
            return new EwiPracaDbContext();
        }

        // Fluent API
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<ApplicationUser>().ToTable("Users").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        }
    }
}
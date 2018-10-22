using EwiPraca.Constants;
using EwiPraca.Encryptor;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace EwiPraca.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<EwiPracaDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = "Migrations";
        }

        protected override void Seed(EwiPracaDbContext context)
        {
            var rolesArray = new[] { RolesNames.Administrator, RolesNames.User, RolesNames.Viewer };

            var administrators = new List<string>
            {
                EncryptionService.EncryptEmail("termbest@gmail.com"),
                EncryptionService.EncryptEmail("witkowskimichal90@gmail.com")
            };

            const string defaultPassword = "EwiPraca321";

            var passwordHash = new PasswordHasher();
            var password = passwordHash.HashPassword(defaultPassword);

            var roles = rolesArray.Select(role => new IdentityRole(role)).ToList();

            foreach (IdentityRole role in roles)
            {
                if (context.Roles.FirstOrDefault(x => x.Name == role.Name) == null)
                {
                    context.Roles.AddOrUpdate(role);
                }
            }

            context.SaveChanges();

            foreach (var user in administrators.Select(admin => new ApplicationUser
            {
                FirstName = EncryptionService.Encrypt("Default name"),
                Surname = EncryptionService.Encrypt("Default surname"),
                UserName = admin,
                Email = admin,
                PasswordHash = password,
                LockoutEnabled = true,
                LastLoginDate = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString()
            }))
            {
                if (context.Users.FirstOrDefault(u => u.UserName == user.UserName) == null)
                {
                    context.Users.Add(user);
                }
            }

            context.SaveChanges();

            var adminRoleId = context.Roles.FirstOrDefault(x => x.Name == RolesNames.Administrator).Id;

            foreach (var admin in administrators)
            {
                var userAdmin = context.Users.FirstOrDefault(x => x.Email == admin);

                var userRole = new IdentityUserRole
                {
                    RoleId = adminRoleId,
                    UserId = userAdmin.Id
                };

                if (userAdmin.Roles.FirstOrDefault(x => x.RoleId == userRole.RoleId) == null)
                {
                    userAdmin.Roles.Add(userRole);
                }
            }

            context.SaveChanges();
        }
    }
}
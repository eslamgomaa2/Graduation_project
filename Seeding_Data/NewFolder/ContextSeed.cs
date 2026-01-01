/*using Domins.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OA.Domain.Auth;
using Seeding_Data.Seeds;
using System.Collections.Generic;

namespace OA.Persistence.Seeds
{
    public static class ContextSeed
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            CreateRoles(modelBuilder);

            CreateDoctorAdminUsers(modelBuilder);

          MapUserRole(modelBuilder);

            AllDoctors(modelBuilder);
        }

        private static void CreateRoles(ModelBuilder modelBuilder)
        {
            List<IdentityRole> roles = DefaultRoles.IdentityRoleList();
            modelBuilder.Entity<IdentityRole>().HasData(roles);

        }

        private static void CreateDoctorAdminUsers(ModelBuilder modelBuilder)
        {
            List<ApplicationUser> users = DefaultUser.DoctorAndAdminAccounts();
            modelBuilder.Entity<ApplicationUser>().HasData(users);
        }

        private static void MapUserRole(ModelBuilder modelBuilder)
        {
            var identityUserRoles = MappingUserRole.IdentityUserRole();
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(identityUserRoles);
        }
        private static void AllDoctors(ModelBuilder modelBuilder) {
            List<Doctor> doctors = DoctorAdminNames.doctors();
            modelBuilder.Entity<Doctor>().HasData(doctors);
        
        }
    }
}
*/
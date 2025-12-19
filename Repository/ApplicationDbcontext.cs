
using Domins.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OA.Domain.Auth;
using OA.Persistence.Seeds;
using Seeding_Data.Seeds;

namespace Repository
{
    public class ApplicationDbcontext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbcontext(DbContextOptions options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = true;
        }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<SensorData> SensorData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging(true);
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);



            modelBuilder.HasDefaultSchema("Identity");


            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
                entity.HasData(DefaultRoles.MyRole);
            });

            modelBuilder.Entity<ApplicationUser>(entity =>
            {

                entity.ToTable(name: "Users");
                entity.HasData(DefaultUsers.GetDefaultUsers);


            }); 

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {

                entity.ToTable("UserRoles");

                entity.HasData(MappingUserRole.IdentityUserRole);

            });


            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {

                entity.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {

                entity.HasKey(o => o.UserId);

                entity.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {

                entity.ToTable("RoleClaims");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {

                entity.ToTable("UserTokens");
            });






        }
      
           
    } 
}

   

using Domains.Entities;
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
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<GroupMessage> GroupMessages { get; set; }
        public DbSet<BlockedUser> BlockedUsers { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging(true);
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);



            modelBuilder.HasDefaultSchema("Identity");

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasOne(e => e.CreatedByUser)
                    .WithMany(u => u.CreatedGroups)
                    .HasForeignKey(e => e.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            
            modelBuilder.Entity<GroupMember>(entity =>
            {
                entity.HasIndex(e => new { e.GroupId, e.UserId }).IsUnique();
                entity.HasOne(e => e.Group)
                    .WithMany(g => g.Members)
                    .HasForeignKey(e => e.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.User)
                    .WithMany(u => u.GroupMemberships)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notifications", "Identity");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.Message)
                    .HasMaxLength(2000);

                entity.Property(e => e.Type)
                    .HasConversion<int>();

                entity.HasOne<ApplicationUser>()
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<GroupMessage>(entity =>
            {
                entity.HasOne(e => e.Group)
                    .WithMany(g => g.Messages)
                    .HasForeignKey(e => e.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Sender)
                    .WithMany(u => u.GroupMessages)
                    .HasForeignKey(e => e.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);
            });




            modelBuilder.Entity<BlockedUser>(entity =>
            {
                entity.HasIndex(e => new { e.BlockedByUserId, e.BlockedUserId }).IsUnique();

                entity.HasOne(e => e.BlockedByUser)
                    .WithMany(u => u.BlockedUsers)  
                    .HasForeignKey(e => e.BlockedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.BlockedUserNavigation)
                    .WithMany(u => u.BlockedByUsers)  
                    .HasForeignKey(e => e.BlockedUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


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

   
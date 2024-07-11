using Microsoft.AspNetCore.Identity;
using OA.Domain.Auth;

namespace Seeding_Data.Seeds
{
    public static class DefaultUsers
    {
        public static List<ApplicationUser> GetDefaultUsers { get; } = new()
        {

                new ()
            {

                Id = "1",
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "Admin",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "Admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "012152001",
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null,"Admin123!")

            },
            new ()
            {
                Id = "2",
                FirstName = "Ambulance",
                LastName = "Ambulance",
                UserName = "Ambulance",
                NormalizedUserName = "AMBULANCE@GMAIL.COM",
                Email = "Ambulance@gmail.com",
                NormalizedEmail = "AMBULANCE@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "123",
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Ambulance123!")

            }


        };


    }
}

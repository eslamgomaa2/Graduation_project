using Microsoft.AspNetCore.Identity;
using OA.Domain.Enum;
using System.Collections.Generic;

namespace OA.Persistence.Seeds
{
    public static class MappingUserRole
    {
        public static List<IdentityUserRole<string>> IdentityUserRole { get; } = new()
        {
           new ()
            {
                    RoleId = "1",UserId = "1"
             },
            new()
            {
                RoleId = "4",UserId = "2"

            }

        };
        
             
        
    }
}

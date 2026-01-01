
using Domains.Entities;
using Domins.Model;
using Microsoft.AspNetCore.Identity;

namespace OA.Domain.Auth
{
    public class ApplicationUser : IdentityUser
    {
        
        
        public string? FirstName { get; set; }
        
        public string? LastName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastSeen { get; set; }
        public bool IsOnline { get; set; } = false;
        public string? Bio { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }

        public virtual ICollection<GroupMember> GroupMemberships { get; set; } = new List<GroupMember>();
        public virtual ICollection<Group> CreatedGroups { get; set; } = new List<Group>();
        public virtual ICollection<GroupMessage> GroupMessages { get; set; } = new List<GroupMessage>();
        public virtual ICollection<BlockedUser> BlockedUsers { get; set; } = new List<BlockedUser>();
        public virtual ICollection<BlockedUser> BlockedByUsers { get; set; } = new List<BlockedUser>();

        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}
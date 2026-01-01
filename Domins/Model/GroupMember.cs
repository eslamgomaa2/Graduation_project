
using OA.Domain.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities
{
    public class GroupMember
    {
        [Key]
        public int Id { get; set; }

        public int GroupId { get; set; }

        [Required]
        [MaxLength(450)]
        public string UserId { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public GroupRole Role { get; set; } = GroupRole.Member;

        public bool IsMuted { get; set; } = false;

        public DateTime? MutedUntil { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
    }

   
}
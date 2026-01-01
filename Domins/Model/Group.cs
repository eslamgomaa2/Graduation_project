
using OA.Domain.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(450)]
        [ForeignKey(nameof(CreatedByUser))]
        public string CreatedByUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        public int MaxMembers { get; set; } = 100;

        public bool IsPrivate { get; set; } = false;
        public virtual ApplicationUser CreatedByUser { get; set; }
        public virtual ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
        public virtual ICollection<GroupMessage> Messages { get; set; } = new List<GroupMessage>();
    }
}
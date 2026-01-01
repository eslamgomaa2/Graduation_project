// Domains/Entities/BlockedUser.cs
using OA.Domain.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities
{
    public class BlockedUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        public string BlockedByUserId { get; set; }

        [Required]
        [MaxLength(450)]
        public string BlockedUserId { get; set; }

        public DateTime BlockedAt { get; set; } = DateTime.UtcNow;

        public string? Reason { get; set; }

        [ForeignKey(nameof(BlockedByUserId))]
        public virtual ApplicationUser BlockedByUser { get; set; }

        [ForeignKey(nameof(BlockedUserId))]
        public virtual ApplicationUser BlockedUserNavigation { get; set; }

    }
}

using OA.Domain.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities
{
    public class GroupMessage
    {
        [Key]
        public int Id { get; set; }

        public int GroupId { get; set; }

        [Required]
        [MaxLength(450)]
        public string SenderId { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsEdited { get; set; } = false;

        public DateTime? EditedAt { get; set; }

        public bool IsDeleted { get; set; } = false;

        public MessageType Type { get; set; } = MessageType.Text;

        [MaxLength(500)]
        public string? AttachmentUrl { get; set; }

        
        [ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; }

        [ForeignKey(nameof(SenderId))]
        public virtual ApplicationUser Sender { get; set; }
    }

   
}
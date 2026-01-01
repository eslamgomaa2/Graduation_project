
using System.ComponentModel.DataAnnotations;

namespace Domains.Dtos
{
    public class CreateGroupDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsPrivate { get; set; } = false;

        [Range(2, 1000)]
        public int MaxMembers { get; set; } = 100;

        public string? ImageUrl { get; set; }
    }
}
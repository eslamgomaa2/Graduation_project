
using System.ComponentModel.DataAnnotations;

namespace Domains.Dtos
{
    public class BlockUserDto
    {
        [Required]
        public string UserIdToBlock { get; set; }

        [StringLength(500)]
        public string? Reason { get; set; }
    }
}
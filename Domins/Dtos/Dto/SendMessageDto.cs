using System.ComponentModel.DataAnnotations;

namespace Domains.Dtos
{
    public class SendMessageDto
    {
        [Required]
        public string GroupName { get; set; }

        [Required]
        [StringLength(5000, MinimumLength = 1)]
        public string Message { get; set; }
    }
}

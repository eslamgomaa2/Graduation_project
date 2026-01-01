using System.ComponentModel.DataAnnotations;

namespace Domins.Dtos.Dto
{
    public class MailRequestdto
    {
        [Required]
        public string ToEmail { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }


    }
}

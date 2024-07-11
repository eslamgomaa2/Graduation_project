using System.ComponentModel.DataAnnotations;

namespace Domins.Dtos.Dto
{
    public class Warningdto
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public string Email { get; set; }
    }
}

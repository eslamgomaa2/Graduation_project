using System.ComponentModel.DataAnnotations;

namespace OA.Domain.Auth
{
    public class RegisterAsDoctorRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        [MinLength(6)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
       
        public string phoneNumber { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        
    }
}

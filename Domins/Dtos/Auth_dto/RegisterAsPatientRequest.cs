using Domins.Model;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domins.Dtos.Auth_dto
{
    public class RegisterAsPatientRequest
    {
        [Required]
        [MaxLength(8)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(8)]
        public string LastName { get; set; }
        [Required]
        [MinLength(8)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
       
      
        public int DoctorId { get; set; }
        public string phoneNumber { get; set; }


        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domins.Dtos.Dto
{
    public class AddDoctorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
        public int PhoneNumber { get; set; }

    }
}

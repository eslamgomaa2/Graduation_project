using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domins.Dtos.Auth_dto
{
    public class ExternalRegisterModel
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Provider { get; set; }
        public string ProviderKey { get; set; }
        [MaxLength(8)]
        public string FirstName { get; set; }
        [MaxLength(8)]
        public string LastName { get; set; }
        [MaxLength(8)]
        public string Username { get; set; }
        
        public string Password { get; set; }
        
    }
}

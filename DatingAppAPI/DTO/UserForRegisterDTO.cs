using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.DTO
{
    public class UserForRegisterDTO
    {
        
        [Required]
        public string UserName { get; set; }
       
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 8 characters")]
        public string Password { get; set; }
    }
}

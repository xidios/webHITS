using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend4.Models
{
    public class RegisterUserMailModel : RegisterUserModel
    {
        [Required]
        public String Email { get; set; }
        public Boolean Exists { get; set; } = false;
        [Required]
        public String Password { get; set; }
        public Boolean Remember { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]

        public String CPassword { get; set; }

    }
}

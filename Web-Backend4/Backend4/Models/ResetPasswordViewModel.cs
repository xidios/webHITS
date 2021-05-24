using System;
using System.ComponentModel.DataAnnotations;

namespace Backend4.Models
{
    public class ResetPasswordViewModel : ResetCodeVerificationViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public String ConfirmPassword { get; set; }
    }
}
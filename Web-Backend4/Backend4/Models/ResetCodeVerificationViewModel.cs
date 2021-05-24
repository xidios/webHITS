using System;
using System.ComponentModel.DataAnnotations;

namespace Backend4.Models
{
    public class ResetCodeVerificationViewModel : ResetViewModel
    {
        [Required]
        public String Code { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend4.Models
{
    public class ResetViewModel
    {
        [Required]
        [EmailAddress]
        public String Email { get; set; }
    }
}

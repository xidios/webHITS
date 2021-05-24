using System;
using System.ComponentModel.DataAnnotations;

namespace Backend2.Models
{
    public class GreetingViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public String Name { get; set; }
    }
}

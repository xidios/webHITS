using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class PatientEditModel
    {
        [Required]
        public String Name { get; set; }
        [Required]
        public String Address { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public String Gender { get; set; }
    }
}

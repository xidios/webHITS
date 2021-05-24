using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class LabCreateModel
    {
        [Required]
        [MaxLength(200)]
        public String Name { get; set; }

        public String Address { get; set; }

        public String Phones { get; set; }
    }
}

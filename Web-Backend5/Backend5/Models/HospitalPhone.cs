using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models
{
    public class HospitalPhone
    {
        public Int32 HospitalId { get; set; }

        public Hospital Hospital { get; set; }

        public Int32 PhoneId { get; set; }

        [Required]
        [MaxLength(20)]
        public String Number { get; set; }
    }
}

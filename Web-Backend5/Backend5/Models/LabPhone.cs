using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models
{
    public class LabPhone
    {
        public Int32 LabId { get; set; }

        public Lab Lab { get; set; }

        public Int32 PhoneId { get; set; }

        [Required]
        [MaxLength(20)]
        public String Number { get; set; }
    }
}

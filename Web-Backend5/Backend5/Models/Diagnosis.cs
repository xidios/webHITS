using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models
{
    public class Diagnosis
    {
        public Int32 Id { get; set; }
        public Int32 PatientId { get; set; }
        [Required]
        public String Type { get; set; }
        [Required]
        public String Complications { get; set; }
        [Required]
        public String Details { get; set; }
        public Patient Patient { get; set; }
    }
}

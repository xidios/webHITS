using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models
{
    public class DoctorPatient
    {
      
        public Int32 DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public Int32 PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}

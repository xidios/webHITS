using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models
{
    public class Doctor
    {
        public Int32 Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Specialty { get; set; }
        public ICollection<HospitalDoctor> Hospitals { get; set; }
        public ICollection<DoctorPatient> Patients { get; set; }

    }
}

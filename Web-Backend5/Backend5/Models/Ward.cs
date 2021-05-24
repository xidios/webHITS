using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models
{
    public class Ward
    {
        public Int32 Id { get; set; }

        public Int32 HospitalId { get; set; }

        public Hospital Hospital { get; set; }
        public ICollection<WardStaf> WardStafs { get; set; }
        public ICollection<Placement> Placements { get; set; }
        [Required]
        [MaxLength(200)]
        public String Name { get; set; }
    }
}

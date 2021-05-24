using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models
{
    public class WardStaf
    {
       
        public Int32 Id { get; set; }
        public Int32 WardId { get; set; }
        public Ward Ward { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Postion { get; set; }
    }
}

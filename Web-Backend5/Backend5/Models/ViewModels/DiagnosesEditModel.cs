using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class DiagnosesEditModel
    {
        [Required]
        public String Type { get; set; }
        [Required]
        public String Complications { get; set; }
        [Required]
        public String Details { get; set; }
    }
}

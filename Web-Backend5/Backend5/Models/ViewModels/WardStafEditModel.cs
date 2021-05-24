using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class WardStafEditModel
    {
        [Required]
        public String Name { get; set; }
        [Required]
        public String Postion { get; set; }
    }
}

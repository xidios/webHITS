using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend4.Models.Controls
{
    public class TextBoxViewModel
    {
        [Required]
        public String Text { get; set; }
    }
}

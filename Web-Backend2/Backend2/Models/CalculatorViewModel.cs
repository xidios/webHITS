using System;
using System.ComponentModel.DataAnnotations;

namespace Backend2.Models
{
    public class CalculatorViewModel
    {
        //[Required(ErrorMessage = "?")]
        public int? Number1 { get; set; }
        public int? Number2 { get; set; }
        public String Operation { get; set; }
        public int? Result { get; set; }
    }
}

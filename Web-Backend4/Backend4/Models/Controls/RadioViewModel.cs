using System;
using System.ComponentModel.DataAnnotations;

namespace Backend4.Models.Controls
{
    public class RadioViewModel
    {
        [Required]
        public Int32? Month { get; set; }
    }
}
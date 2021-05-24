using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend4.Models
{
    public class RegisterUserModel
    {
        
        public List<Person> Persons { get; set; } = new List<Person>();
        [Required]
        public String FirstName { get; set; }
        [Required]
        public String SecondName { get; set; }
        public Int32 Day { get; set; }
        public String Month { get; set; }//id better
        public Int32 Year { get; set; }
        public String Gender { get; set; }//enum 
        //Birthday btd { get; set; }
    }
}

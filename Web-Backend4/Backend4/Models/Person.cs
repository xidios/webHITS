using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend4.Models
{
    public class Person
    {
        public String FirstName { get; set; }
        public String SecondName { get; set; }
        public Int32 Day { get; set; }
        public String Month { get; set; }
        public Int32 Year { get; set; }
        public String Gender { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public Boolean Remeber { get; set; }
        public Person() { }
        public bool Equals(RegisterUserModel p)
        {
            if (this.FirstName == p.FirstName &&
                this.SecondName == p.SecondName &&
                this.Day == p.Day &&
                this.Month == p.Month &&
                this.Year == p.Year &&
                this.Gender == p.Gender)
                return true;
            else return false;
        }
        public bool EqualsEmail(RegisterUserMailModel model)
        {
            if (this.FirstName == model.FirstName &&
                this.SecondName == model.SecondName &&
                this.Day == model.Day &&
                this.Month == model.Month &&
                this.Year == model.Year &&
                this.Gender == model.Gender &&
                this.Email == model.Email &&
                this.Password == model.Password) return true;
            else return false;
        }
    }
}

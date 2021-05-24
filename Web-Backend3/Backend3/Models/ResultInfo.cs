using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend3.Models
{
    public class ResultInfo
    {
        public Int32 Number1 { get; set; }
        public Int32 Number2 { get; set; }
        public string Operation { get; set; }
        public Int32 Answer { get; set; }
        public bool Equal { get; set; }
        public ResultInfo() { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend3.Models
{
    public class QuizViewModel
    {
        public Int32 Number1 { get; set; }
        public Int32 Number2 { get; set; }
        public string Operation { get; set; }
        public Int32 Result { get; set; }
        public Int32 Answer { get; set; }
        public List<ResultInfo> ListOfResults { get; set; } = new List<ResultInfo>();
        public QuizViewModel() { }
    }
}

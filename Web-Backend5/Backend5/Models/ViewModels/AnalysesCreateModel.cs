using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class AnalysesCreateModel
    {
        public String Type { get; set; }
        public DateTime Date { get; set; }
        public String Status { get; set; }
        public Int32 LabId { get; set; }
        public Lab Lab { get; set; }
    }
}

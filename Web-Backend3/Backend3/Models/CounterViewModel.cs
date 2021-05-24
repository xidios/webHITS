using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend3.Models
{
    public class CounterViewModel
    {
        public Int32 CurrentCount { get; set; }

        public List<CounterAction> Actions { get; set; } = new List<CounterAction>();
    }
}

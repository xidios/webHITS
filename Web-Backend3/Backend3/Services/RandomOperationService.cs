using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend3.Models;

namespace Backend3.Services
{
    public class RandomOperationService : IRandomOperationService
    {
        Random r = new Random();
        public Operation RandomOperation()
        {
            Operation o = (Operation)r.Next(0, 4);
            return o;
        }
        public string CheckOperation(Operation op)
        {
            switch (op)
            {
                case Operation.Addition:
                    return "+";
                case Operation.Subtraction:
                    return "-";
                case Operation.Multiplication:
                    return "*";
                case Operation.Division:
                    return "/";
                default:
                    throw new ArgumentOutOfRangeException(nameof(op), op, null);
            }
        }
    }
}

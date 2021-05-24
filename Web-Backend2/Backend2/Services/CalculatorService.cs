using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend2.Services
{
    public class CalculatorService : ICalculatorService
    {
        public int Calculating(int number1, int number2, String operation)
        {
            if (operation == "+")
                return number1 + number2;
            if (operation == "-")
                return number1 - number2;
            if (operation == "*")
                return number1 * number2;
            if (operation == "/")
                return number1 / number2;
            return 0;
        }
    }
}

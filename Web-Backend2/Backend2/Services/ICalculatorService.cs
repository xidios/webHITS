using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend2.Services
{
    public interface ICalculatorService
    {
        int Calculating(int number1,int number2,String operation);
    }
}

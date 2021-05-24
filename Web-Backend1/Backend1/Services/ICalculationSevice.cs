using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend1.Services
{
    public interface ICalculationSevice
    {
        int Addition(int r1, int r2);

        int Subtraction(int r1, int r2);

        int Multiplication(int r1, int r2);

        double Division(int r1, int r2);
        
    }
}

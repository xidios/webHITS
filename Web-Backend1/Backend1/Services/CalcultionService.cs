using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend1.Services
{
    public class CalcultionService : ICalculationSevice
    {
        
        public int Addition(int r1,int r2) => r1 + r2;

        public int Subtraction(int r1, int r2) => r1 - r2;

        public int Multiplication(int r1, int r2) =>  r1* r2;

        public double Division(int r1, int r2) => (double)r1 / r2; //try catch throw zero

    }
}

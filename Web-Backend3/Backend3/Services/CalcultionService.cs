using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend3.Models;

namespace Backend3.Services
{
    public class CalcultionService : ICalculationSevice
    {
        
        private int Addition(int r1,int r2) => r1 + r2;

        private int Subtraction(int r1, int r2) => r1 - r2;

        private int Multiplication(int r1, int r2) =>  r1* r2;

        private int Division(int r1, int r2) => r1 / r2; //try catch throw zero

        public int Calculation(int r1,int r2,Operation operation)
        {
            switch (operation)
            {
                case Operation.Addition:
                    return Addition(r1, r2);
                case Operation.Subtraction:
                    return Subtraction(r1,r2);
                case Operation.Multiplication:
                    return Multiplication(r1,r2);
                case Operation.Division:
                    return Division(r1,r2);
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
            }
        }

    }
}

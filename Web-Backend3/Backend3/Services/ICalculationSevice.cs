using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend3.Models;

namespace Backend3.Services
{
    public interface ICalculationSevice
    {
        //int Addition(int r1, int r2);

        //int Subtraction(int r1, int r2);

        //int Multiplication(int r1, int r2);

        //int Division(int r1, int r2);

        int Calculation(int r1, int r2, Operation operation);


    }
}

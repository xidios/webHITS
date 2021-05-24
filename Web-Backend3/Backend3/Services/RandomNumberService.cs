using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend3.Services
{
    public class RandomNumberService : IRandomNumberService
    {
        private Random generator { get; set; } = new Random();
        private const int lower = 1;
        private const int upper = 21;
        public int GenerateNumber()
        {
            int random_number = generator.Next(lower, upper);
            return random_number;
        }

    }
}

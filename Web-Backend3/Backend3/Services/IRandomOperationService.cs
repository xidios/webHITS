using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend3.Models;

namespace Backend3.Services
{
    public interface IRandomOperationService
    {
        Operation RandomOperation();
        string CheckOperation(Operation op);
    }
}

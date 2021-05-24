using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend3.Controllers
{
    public class MockupsController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Counter()
        {
            return this.View();
        }

        public IActionResult CounterResult()
        {
            return this.View();
        }

        public IActionResult Quiz()
        {
            return this.View();
        }

        public IActionResult QuizResult()
        {
            return this.View();
        }
    }
}

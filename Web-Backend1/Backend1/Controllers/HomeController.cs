using Backend1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Error()
        {
            return this.View();
        }

        
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Backend4.Controllers
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

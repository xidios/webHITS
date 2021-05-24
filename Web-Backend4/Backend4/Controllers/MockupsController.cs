using Microsoft.AspNetCore.Mvc;

namespace Backend4.Controllers
{
    public class MockupsController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult SignUp()
        {
            return this.View();
        }

        public IActionResult SignUpAlreadyExists()
        {
            return this.View();
        }

        public IActionResult SignUpCredentials()
        {
            return this.View();
        }

        public IActionResult SignUpResult()
        {
            return this.View();
        }
    }
}
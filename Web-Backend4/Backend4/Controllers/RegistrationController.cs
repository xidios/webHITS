using Backend4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend4.Services;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Backend4.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IUserInfoCheckService userInfoCheckService;
        
        public RegistrationController(IUserInfoCheckService userInfoCheckService)
        {
            this.userInfoCheckService = userInfoCheckService;
           
        }        


        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult SignUp()
        {
            this.ViewBag.AllMonths = this.GetAllMonths();
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(RegisterUserModel model)
        {
            this.ViewBag.AllMonths = this.GetAllMonths();
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            if (model.FirstName == null)//req
                this.ModelState.AddModelError("FirstName", "First Name is requared");
            if (model.SecondName == null)
                this.ModelState.AddModelError("SecondName", "Second Name is requared");
            if (model.Gender == null)
                this.ModelState.AddModelError("Gender", "Gender is requared");
            if (this.ModelState.ErrorCount == 0)
            {
                if (userInfoCheckService.CheckUserInfo(ref model))
                    return this.View("SignUpVerification", model);


                return this.View("SignUpEmail", new RegisterUserMailModel
                {
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    Day = model.Day,
                    Month = model.Month,
                    Year = model.Year,
                    Gender = model.Gender
                });
            }
            return this.View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUpVerification(RegisterUserModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            return this.View("SignUpEmail", new RegisterUserMailModel
            {
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                Day = model.Day,
                Month = model.Month,
                Year = model.Year,
                Gender = model.Gender
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUpEmail(RegisterUserMailModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            if (model.Email == null)
            {
                this.ModelState.AddModelError("Email", "Email is requared");
            }
            if (model.Password == null)
            {
                this.ModelState.AddModelError("Password", "Password is requared");
            }            
            if (this.ModelState.ErrorCount == 0)
            {                
                if (userInfoCheckService.CheckUserExists(ref model))
                {
                    model.Exists = true;                    
                    return this.View("SignUpResult", model);
                }                
                userInfoCheckService.Persons.Add(new Person
                {
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    Day = model.Day,
                    Month = model.Month,
                    Year = model.Year,
                    Gender = model.Gender,
                    Email = model.Email,
                    Password = model.Password,
                    Remeber = model.Remember
                });
            }
            return this.View("SignUpResult", model);
        }

        private Month[] GetAllMonths()
        {
            return CultureInfo.InvariantCulture.DateTimeFormat.MonthNames
                .Select((x, i) => new Month { Id = i + 1, Name = x })
                .Where(x => !String.IsNullOrEmpty(x.Name))
                .ToArray();
        }

    }
}

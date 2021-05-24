using System;
using Backend2.Models;
using Backend2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend2.Controllers
{
    public class GreetingController : Controller
    {
        private readonly IGreetingService greetingService;

        public GreetingController(IGreetingService greetingService)
        {
            this.greetingService = greetingService;
        }

        public ActionResult Manual()
        {
            if (this.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                String name = this.Request.Form["Name"];
                if (String.IsNullOrEmpty(name))
                {
                    this.ViewBag.Error = "Name is required";
                    return this.View();
                }

                if (name.ToLowerInvariant().Contains("admin"))
                {
                    this.ViewBag.Error = "You are not admin";
                    return this.View();
                }

                var greeting = this.greetingService.GetGreeting(name);
                var resultModel = new GreetingResultViewModel
                {
                    Greeting = greeting
                };

                return this.View("Result", resultModel);
            }

            return this.View();
        }

        public ActionResult ManualWithSeparateHandlers()
        {
            return this.View();
        }

        [HttpPost, ActionName("ManualWithSeparateHandlers")]
        [ValidateAntiForgeryToken]
        public ActionResult ManualWithSeparateHandlersConfirm()
        {
            String name = this.Request.Form["Name"];
            if (String.IsNullOrEmpty(name))
            {
                this.ViewBag.Error = "Name is required";
                return this.View();
            }

            if (name.ToLowerInvariant().Contains("admin"))
            {
                this.ViewBag.Error = "You are not admin";
                return this.View();
            }

            var greeting = this.greetingService.GetGreeting(name);
            var resultModel = new GreetingResultViewModel
            {
                Greeting = greeting
            };

            return this.View("Result", resultModel);
        }

        public ActionResult ModelBindingInParameters()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModelBindingInParameters(String name)
        {
            if (String.IsNullOrEmpty(name))
            {
                this.ViewBag.Error = "Name is required";
                return this.View();
            }

            if (name.ToLowerInvariant().Contains("admin"))
            {
                this.ViewBag.Error = "You are not admin";
                return this.View();
            }

            var greeting = this.greetingService.GetGreeting(name);
            var resultModel = new GreetingResultViewModel
            {
                Greeting = greeting
            };

            return this.View("Result", resultModel);
        }

        public ActionResult ModelBindingInSeparateModel()
        {
            return this.View(new GreetingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModelBindingInSeparateModel(GreetingViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                if (model.Name.ToLowerInvariant().Contains("admin"))
                {
                    this.ModelState.AddModelError("Name", "You are not admin");
                    return this.View(model);
                }

                var greeting = this.greetingService.GetGreeting(model.Name);
                var resultModel = new GreetingResultViewModel
                {
                    Greeting = greeting
                };

                return this.View("Result", resultModel);
            }

            return this.View(model);
        }
    }
}

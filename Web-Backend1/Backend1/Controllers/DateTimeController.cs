using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend1.Models;
using Backend1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend1.Controllers
{
    public class DateTimeController : Controller
    {
        private readonly IDateTimeService dataTimeService;

        public DateTimeController(IDateTimeService dataTimeService)
        {
            this.dataTimeService = dataTimeService;
        }

        public ActionResult PassUsingViewData()
        {
            var current = this.dataTimeService.Now;
            this.ViewData["Current"] = current;

            return this.View();
        }

        public ActionResult PassUsingViewBag()
        {
            var current = this.dataTimeService.Now;
            this.ViewBag.Current = current;

            return this.View();
        }

        public ActionResult PassUsingModel()
        {
            var current = this.dataTimeService.Now;
            var model = new DateTimeViewModel
            {
                Current = current
            };

            return this.View(model);
        }

        public ActionResult AccessServiceDirectly()
        {
            return this.View();
        }
        
    }
}
